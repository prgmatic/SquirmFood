<?php
require('Connect.php');

$message = "";
$message .= "Connected to database...\r\n";
$levelInfo;

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $levelName     = StripInput($_POST["LevelName"]);
    $authorKey     = StripInput($_POST["AuthorKey"]);
    $levelData     = StripInput($_POST["LevelData"]);
    $levelID       = StripInput($_POST["LevelID"]);
    
    $message .= "$authorKey\r\n";
                                
    if($db->GetPlayer($authorKey) != null)
    {
        if($db->GetLevel($authorKey, $levelID) != null)
        {
            $message .= "Got level";
            if($db->UpdateLevel($levelID, $authorKey, $levelData))
                $message .= "Level Updated...\r\n";
            else
                $message .= $db->GetError();
        }
        else
        {
            $levelID = $db->AddLevel($levelName, $authorKey, $levelData);
            if($levelID > -1)
                $message .= "Your level has been added\r\n";
            else
                $message .= $db->GetError();
        }
    }
    else $message .= "Your key is not valid.\r\n";
    
    
    $levelInfo['LevelName'] = $levelName;
    $levelInfo['ID'] = $levelID;
}

$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'LevelInfo' => $levelInfo
);
echo json_encode($response);
