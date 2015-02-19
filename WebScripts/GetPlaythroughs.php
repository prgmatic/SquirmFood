<?php
require('Connect.php');

$rows = array();
$message = "";
$message .= "Connected to database...\r\n";

if($_SERVER["REQUEST_METHOD"] == "POST")
{
    $sceneGUID     = StripInput($_POST["SceneGUID"]);
    $layoutGUID    = StripInput($_POST["LayoutGUID"]);
    $accessPassword      = StripInput($_POST["Password"]);
    
    if($accessPassword == $expectedAccessPassword)
    {
        $message .= "$sceneGUID\r\n$layoutGUID\r\n";

        $query = "SELECT * FROM Playthroughs WHERE SceneGUID = '$sceneGUID' AND LayoutGUID = '$layoutGUID'";
        $queryResult = $conn->query($query);

        if($queryResult)
        {
            while($row = $queryResult->fetch_assoc())
            {
                unset($row["Actions"]);
                $rows[] = $row;
            }
        }
        else
        {
            $message .= "No Playthroughs for this layout\r\n";
        }
    }
    else
        $message .= "Password Invalid";
}
mysqli_close($conn);
$message .= "Closing Connection...Done";

$response = array(
            'Message' => $message,
            'Playthroughs' => $rows
    );
echo json_encode($response);
?>