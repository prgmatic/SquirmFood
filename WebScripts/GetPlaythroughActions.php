<?php
require('Connect.php');

$actions;
$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key                 = StripInput($_POST["Key"]);
    $password            = StripInput($_POST["Password"]);
    $playthroughID       = StripInput($_POST["PlaythroughID"]);

    $actions = $db->GetPlaythroughActions($key, $password, $playthroughID);
    if($actions != null)
        $message .= "Got playthrough actions";
    else 
        $message .= $db->GetError() . "\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'Actions' => $actions
);
echo json_encode($response);
?>