<?php
require('Connect.php');

$myLevels = null;
$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $key = StripInput($_POST["Key"]);

    if($db->GetPlayer($key) != null)
    {    
        $myLevels = $db->GetMyLevels($key);
        if($myLevels != null)
            $message .= "Obtained my levels\r\n";
        else
            $message .= $db->GetError();
    }
    else
        $message .= "Your key is invalid.";
}
$db->CloseConnection();
$message .= "Closing Connection...Done";

$response = array(
    'Message' => $message,
    'Levels' => $myLevels
);
echo json_encode($response);
?>