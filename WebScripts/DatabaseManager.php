<?php

class DatabaseManager
{
    
    public $error = "";
    private $conn;
    const AccessPassword = "GoTeam";
    
    //Constructor
    function __construct($conn)
    {
        $this->conn = $conn;
    }
    
    //Queries
    public function GetPlayer($key)
    {
        $query = "SELECT * FROM Players WHERE `Key`= '$key'";
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            while($row = $queryResult->fetch_assoc())
            {
                return $this->ArrayToObject($row);
            }
        }
        $this->error = $this->conn->error;
        return null;
    }
    public function GetLevel($key, $id, $limitedInfo = false)
    {
        $player = $this->GetPlayer($key);
        if($player == null)
        {
            $this->error = "Key invalid";
            return null;
        }
        
        $query = "SELECT * FROM Levels WHERE id = $id";        
        if(!$player->Admin)$query .= " AND AuthorKey = $key";
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            while($row = $queryResult->fetch_assoc())
            {
                if($limitedInfo)
                {
                    unset($row["AuthorName"]);
                    unset($row["AuthorKey"]);
                    unset($row["Version"]);
                    unset($row["TimeCreated"]);
                    unset($row["TimeUpdated"]);
                    unset($row["PlaythroughID"]);
                    unset($row["IsSubmission"]);
                    unset($row["Public"]);
                }
                return $this->ArrayToObject($row);
            }
        }
        $this->error = $this->conn->error;
        return null;
    }
    public function GetMyLevels($key)
    {
        if($this->GetPlayer($key) == null) return null;
        
        $query = "SELECT id, LevelName FROM Levels WHERE AuthorKey = '$key' AND IsSubmission = 0";
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            $levels = array();
            while($row = $queryResult->fetch_assoc())
                $levels[] = $row;
            return $this->ArrayToObject($levels);
        }
        return null;
    }
    public function GetAllLevels($key, $password, $includeLevelData = false, $publicOnly = false, $limitedInfo = false)
    {
        global $message;
        if($password != self::AccessPassword) 
        {
            $this->error = "Invalid password";
            return null;
        }
        $player = $this->GetPlayer($key);
        if($player == null || !$player->Admin)
        {
            $this->error = "Invalid Key or you are not and admin\r\n";
            return null;
        }
        $query = "SELECT Levels.*, Players.Name AS AuthorName, 
        COUNT(Playthroughs.LevelID) as Playthroughs,
        AVG(Playthroughs.Difficulty) as AverageDifficulty,
        AVG(Playthroughs.Satisfaction) as AverageSatisfaction,
        AVG(Playthroughs.Duration) as AverageSolveTime
        FROM Levels 
        INNER JOIN Players ON Levels.AuthorKey=Players.Key
        LEFT JOIN Playthroughs ON Levels.id=Playthroughs.LevelID
        GROUP BY Levels.id";
        if($publicOnly) $query .= " WHERE Public = 1";
        
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            $levels = array();
            while($row = $queryResult->fetch_assoc())
            {
                
                if(!isset($row["AuthorName"])) // If author name is not set
                {
                    $row["AuthorName"] = $row["Name"]; // Rename row.Name to row.AuthorName
                    unset($row["Name"]);
                }
                if($limitedInfo) // Strip fields if only looking for limited info
                {
                    unset($row["AuthorName"]);
                    unset($row["AuthorKey"]);
                    unset($row["Version"]);
                    unset($row["TimeCreated"]);
                    unset($row["TimeUpdated"]);
                    unset($row["PlaythroughID"]);
                    unset($row["IsSubmission"]);
                    unset($row["Public"]);
                    unset($row["Admin"]);
                    unset($row["Key"]);
                }
                if(!$includeLevelData) // Strip Level Data if not required
                    unset($row["LevelData"]);
                $levels[] = $row;
            }
            //$message .= var_export($levels, true);
            return $this->ArrayToObject($levels);
        }
        return null;
    }
    public function AddLevel($levelName, $authorKey, $levelData, $authorName = "", $isSubmission = false)
    {
        global $message;
        $time = time();
        $isSubmission = (int)$isSubmission;
        $query = "INSERT INTO Levels (LevelName, AuthorName, AuthorKey, LevelData, IsSubmission, TimeCreated)
        VALUES('$levelName', '$authorName', '$authorKey', '$levelData', $isSubmission, $time)";
        $message .= "Query: $query\r\n";
        if($this->conn->query($query))
        {
            return $this->conn->insert_id;
        }
        else
        {
            $this->error = $this->conn->error;
            return -1;
        }
    }
    public function UpdateLevel($levelID, $authorKey, $levelData)
    {
        $time = time();
        $query = "UPDATE Levels SET 
        LevelData = '$levelData', 
        TimeUpdated = $time,
        Version = Version + 1
        WHERE AuthorKey = '$authorKey' AND id = $levelID AND LevelData <> '$levelData'";
        if($this->conn->query($query))
        {
            return true;
        }
        else
            return false;
    }
    public function AddPlaythrough($levelID, $testerName, $testerKey, $totalMoves, $movesOnWin, $difficulty, $satisfaction, $notes, $duration, $actions)
    {
        if($this->GetPlayer($testerKey) != null)
        {
            $level = $this->GetLevel($testerKey, $levelID);
            if($level == null) return -1;

            $version = $level->Version;
            $ip = $this->GetClientIP();
            $time = time();
            if($difficulty == null) $difficulty = "NULL";
            if($satisfaction == null)$satisfaction = "NULL";
            
            $query = "INSERT INTO Playthroughs (LevelID, Version, TesterName, TesterKey, IP, TotalMoves, MovesOnWin, Difficulty, Satisfaction, Notes, Duration, Time, Actions)
            VALUES($levelID, $version, '$testerName', '$testerKey', '$ip', $totalMoves, $movesOnWin, $difficulty, $satisfaction, '$notes', $duration, $time, '$actions')";
            if($this->conn->query($query))
            {
                return $this->conn->insert_id;
            }
        }
        return -1;
    }
    public function AddFeedbackToPlaythrough($testerKey, $id, $difficulty, $satisfaction, $notes)
    {
        $query = "UPDATE Playthroughs SET Difficulty = $difficulty, Satisfaction = $satisfaction, Notes = '$notes' WHERE id = $id AND TesterKey = '$testerKey'";
        if($this->conn->query($query))
            return true;
        else
            return false;
    }
    public function GetPlaythroughsForLevel($key, $password, $levelID)
    {
        global $message;
        if($password != self::AccessPassword) 
        {
            $this->error = "Invalid password";
            return null;
        }
        $player = $this->GetPlayer($key);
        if($player == null || !$player->Admin)
        {
            $this->error = "Key invalid or you are not an Admin";
            return null;
        }

        $query = "SELECT * FROM Playthroughs WHERE LevelID = '$levelID'";
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            $playthroughs = array();
            while($row = $queryResult->fetch_assoc())
            {
                unset($row["Actions"]);
                $playthroughs[] = $row;
            }
            return $this->ArrayToObject($playthroughs);
        }
        $this->error = $this->conn->error;
        return null;
    }
    public function GetPlaythrough($key, $password, $playthroughID, $includActions = false)
    {
        global $message;
        if($password != self::AccessPassword) 
        {
            $this->error = "Invalid password";
            return null;
        }
        $player = $this->GetPlayer($key);
        if($player == null || !$player->Admin)
        {
            $this->error = "Key invalid or you are not an Admin";
            return null;
        }
        $query = "SELECT * FROM Playthroughs WHERE id = $playthroughID";
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            $level;
            while($row = $queryResult->fetch_assoc())
            {
                if(!$includActions)
                    unset($row["Actions"]);
                $level = $row;
            }
            return $this->ArrayToObject($level);
        }
        $this->error = $this->conn->error;
        return null;
    }
    public function GetPlaythroughActions($key, $password, $playthroughID)
    {
        global $message;
        if($password != self::AccessPassword) 
        {
            $this->error = "Invalid password";
            return null;
        }
        $player = $this->GetPlayer($key);
        if($player == null || !$player->Admin)
        {
            $this->error = "Key invalid or you are not an Admin";
            return null;
        }
        $query = "SELECT Actions FROM Playthroughs WHERE id = $playthroughID";
        $queryResult = $this->conn->query($query);
        if($queryResult)
        {
            $actions = array();
            while($row = $queryResult->fetch_assoc())
            {
                $actions = $row["Actions"];
            }
            return $this->ArrayToObject($actions);
        }
        $this->error = $this->conn->error;
        return null;
    }
    public function GetPlaythroughActionsAndLevel($key, $password, $playthroughID)
    {
        global $message;
        if($password != self::AccessPassword) 
        {
            $this->error = "Invalid password";
            return null;
        }
        $player = $this->GetPlayer($key);
        if($player == null || !$player->Admin)
        {
            $this->error = "Key invalid or you are not an Admin";
            return null;
        }
        $playthrough = $this->GetPlaythrough($key, $password, $playthroughID, true);
        if($playthrough != null)
        {
            $levelID = $playthrough->LevelID;
            $level = $this->GetLevel($key, $levelID);
            if($level != null)
            {
                $rValue = array();
                $rValue["LevelData"] = $level->LevelData;
                $rValue["Actions"] = $playthrough->Actions;
                return $this->ArrayToObject($rValue);
            }
            return null;
        }
        return null;
    }
    public function SubmitLevel($levelName, $authorName, $authorKey, $levelData, $totalMoves, $movesOnWin, $duration, $actions)
    {
        global $message;
        $levelID = $this->AddLevel($levelName, $authorKey, $levelData, $authorName, true);
        if($levelID > -1)
        {
            $playthroughID = $this->AddPlaythrough($levelID, $authorName, $authorKey, $totalMoves, $movesOnWin, null, null, "", $duration, $actions);
            if($playthroughID > -1)
            {
                $query = "UPDATE Levels SET PlaythroughID = $playthroughID WHERE id = $levelID";
                if($this->conn->query($query))
                {
                    
                }
                else $message .= "Could not update playthrough id\r\n";
            }
            else $message .= "Could not get playthrough";
        }
        else $message .= "Could not get level id";
        
    }
    
    // Helper Methods
    public function GetError()
    {
        if(strlen($this->error) > 0) return $this->error;
        return $this->conn->error;   
    }
    public function CloseConnection()
    {
        $this->conn->close();
    }
    private function ArrayToObject($array)
    {
        return json_decode(json_encode($array), FALSE);
    }
    function GetClientIP() 
    {
        $ipaddress = '';
        if (getenv('HTTP_CLIENT_IP'))
            $ipaddress = getenv('HTTP_CLIENT_IP');
        else if(getenv('HTTP_X_FORWARDED_FOR'))
            $ipaddress = getenv('HTTP_X_FORWARDED_FOR');
        else if(getenv('HTTP_X_FORWARDED'))
            $ipaddress = getenv('HTTP_X_FORWARDED');
        else if(getenv('HTTP_FORWARDED_FOR'))
            $ipaddress = getenv('HTTP_FORWARDED_FOR');
        else if(getenv('HTTP_FORWARDED'))
           $ipaddress = getenv('HTTP_FORWARDED');
        else if(getenv('REMOTE_ADDR'))
            $ipaddress = getenv('REMOTE_ADDR');
        else
            $ipaddress = 'UNKNOWN';
        return $ipaddress;
    }
}

?>