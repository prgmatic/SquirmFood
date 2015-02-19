<?php
//Variables for connecting to your database.
//These variable values come from your hosting account.
$hostname = "MonsterMashup.db.9807853.hostedresource.com";
$username = "MonsterMashup";
$dbname = "MonsterMashup";

//These variable values need to be changed by you before deploying
$password = "Screwu91!";
$usertable = "MonsterMashup";
$expectedAccessPassword = "GoTeam";

//Connecting to your database
$conn = new mysqli($hostname, $username, $password, $usertable);
//mysqli_select_db($conn, $usertable);
if ($conn->connect_error) 
{
    die("Connection failed: " . $conn->connect_error);
} 
function StripInput($data)
{
    $data = trim($data);
    $data = stripslashes($data);
    $data = htmlspecialchars($data);
    return $data;	
}
?>