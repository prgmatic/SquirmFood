<?php
require('Connect.php');

$url = "http://pennyanfootballpool.com/MonsterMashup/Build/Build.html?key=";

$message = "";
$message .= "Connected to database...\r\n";

$success = "";
$error = "";
$key = "";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $playerName           = StripInput($_POST["name"]);
    $accessPassword      = StripInput($_POST["password"]);
    
    if($accessPassword == $expectedAccessPassword)
    {
        $key = uniqid();
        $query = "INSERT INTO Players (Name, `Key`) 
        VALUES('$playerName' ,'$key')";

        if($conn->query($query))
            $success = "Player succesfully added to database...\r\n";
        else
            $error = "Error: " . $conn->error . "\r\n";
    }
    else
        $error = "Password Invalid";
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
    "success" => $success,
    "error" => $error,
    "key" => $url . $key
);
    
echo json_encode($response);
?>