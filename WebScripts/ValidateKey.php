<?php
require('Connect.php');

$message = "";
$message .= "Connected to database...\r\n";

$validated = false;
$name = "";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key = StripInput($_POST["Key"]);
    
    $message .= "$key\r\n";
    $player = $db->GetPlayer($key);
    if($player != null)
    {
        $validated = true;
        $name = $player->Name;
    }
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    "Message" => $message,
    "Validated" => $validated,
    "Name" => $name,
    "Error" => $db->error
);
    
echo json_encode($response);
?>