<?php
require('Connect.php');

$message = "";
$message .= "Connected to database...\r\n";
$levels = null;

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key              = StripInput($_POST["Key"]);
    $password         = StripInput($_POST["Password"]);
    
    $levels = $db->GetAllLevels($key, $password);
    if($levels != null)
        $message .= "Got Levels";
    else
        $message .= "Error: " . $db->GetError() . "\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'Levels' => $levels
);
echo json_encode($response);
?>