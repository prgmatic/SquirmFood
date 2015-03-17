<?php
require('Connect.php');

$message = "";
$message .= "Connected to database...\r\n";
$level;      
$levelFound = false;

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key          = StripInput($_POST["Key"]);
    $levelID       = StripInput($_POST["LevelID"]);
    
    $level = $db->GetLevel($key, $levelID, true);
    if($level != null)
        $levelFound = true;
    else 
        $message .= $db->GetError() . "\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";


$response = array(
    'Message' => $message,
    'Level' => $level,
    'LevelFound' => $levelFound
);
echo json_encode($response);
