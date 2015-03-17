<?php
require('Connect.php');

$rows = array();
$message = "";
$message .= "Connected to database...\r\n";
$playthroughs;

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key           = StripInput($_POST["Key"]);
    $password      = StripInput($_POST["Password"]);
    $levelID       = StripInput($_POST["LevelID"]);
    
    $playthroughs = $db->GetPlaythroughsForLevel($key, $password, $levelID);
    if(playthroughs != null)
        $message .= "Got Playthroughs\r\n";
    else
        $message .= $db->GetError() . "\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'Playthroughs' => $playthroughs
);
echo json_encode($response);
?>