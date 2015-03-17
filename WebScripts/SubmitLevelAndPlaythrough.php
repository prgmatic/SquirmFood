<?php
require('Connect.php');

$message = "";
$levelID = -1;
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $authorName    = StripInput($_POST["AuthorName"]);
    $authorKey     = StripInput($_POST["AuthorKey"]);
    $levelID       = StripInput($_POST["LevelID"]);
    
    $totalMoves    = StripInput($_POST["TotalMoves"]);
    $movesOnWin    = StripInput($_POST["MovesOnWin"]);
    $duration      = StripInput($_POST["Duration"]);
    $actions       = StripInput($_POST["Actions"]);
    
    $ip = GetClientIP();
    $time = time();
    
    
    
    $message .= "$authorName
$authorKey\r\n";
    
    $query = "SELECT * FROM Players WHERE 'Key' = '$authorKey'";
    if($conn->query($query))
    {
        $query = "INSERT INTO Levels (AuthorName, AuthorKey, LevelData)
        VALUES('$authorName', '$authorKey', '$levelData')";

        if($conn->query($query))
        {
            $levelID = $conn->insert_id;
            $message .= "Level succesfully added to database...\r\n";
            
            $query = "INSERT INTO Playthroughs (TesterName, IP, LevelID, TotalMoves, MovesOnWin, Difficulty, Satisfaction, Notes, Duration, Time, Actions)
            VALUES('$testerName', '$ip', $levelID, $totalMoves, $movesOnWin, $difficulty, $satisfaction, '$notes', $duration, $time, '$actions')";
            if($conn->query($query))
            {
                $message .= "Playthrough succesfully added to database...\r\n";
            }
            else
                $message .= "Error: " . $conn->error . "\r\n";
        }
        else
            $message .= "Error: " . $conn->error . "\r\n";
    }
    else
        $message .= "Your key is not valid.";
    
    
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message
    );
echo json_encode($response);

function GetClientIP() {
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