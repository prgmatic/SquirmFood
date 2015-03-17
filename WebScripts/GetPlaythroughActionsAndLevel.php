<?php
require('Connect.php');

$result;
$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key                 = StripInput($_POST["Key"]);
    $password            = StripInput($_POST["Password"]);
    $playthroughID       = StripInput($_POST["PlaythroughID"]);

    $result = $db->GetPlaythroughActionsAndLevel($key, $password, $playthroughID);
    if($result != null)
    {
        $message .= "Got playthrough actions";
    }
    else 
        $message .= $db->GetError() . "\r\n";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'Actions' => $result->Actions,
    'LevelData' => $result->LevelData
);
echo json_encode($response);
?>