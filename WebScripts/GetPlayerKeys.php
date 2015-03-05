<?php
require('Connect.php');

$url = "http://pennyanfootballpool.com/MonsterMashup/Build/Build.html?key=";

$players = array();
$error = "";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $accessPassword = StripInput($_POST["password"]);
    
    if($accessPassword == $expectedAccessPassword)
    {
        $query = "SELECT * FROM Players";
        $queryResult = $conn->query($query);

        if($queryResult)
        {
            while($row = $queryResult->fetch_assoc())
            {
                $row["Key"] = $url . $row["Key"];
                $players[] = $row;
            }
        }
        else
        {
            $error = "Error: " . $conn->error . "\r\n";
        }
    }
    else
        $error .= "Password Invalid";
}
mysqli_close($conn);

$response = array(
            'error' => $error,
            'players' => $players
    );
echo json_encode($response);
?>