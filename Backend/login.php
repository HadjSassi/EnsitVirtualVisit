<?php 
include 'cors.php';
$servername = "localhost";
$username = "root";
$password = "Magali_1984";
$dbname = "unitytuto";


//variables submitted by user
$loginUser= $_POST["loginUser"] ;
$loginPass= $_POST["loginPass"];

//create connection 
$conn = new mysqli($servername,$username,$password, $dbname);

//check connection
if($conn -> connect_error){
    die("Connection Failed: ".$conn -> connect_error);
}

$sql = "select password from users where username = '".$loginUser."'";

$result = $conn -> query($sql);

if ($result->num_rows > 0){
    while($row = $result -> fetch_assoc()){
        if($row["password"] == $loginPass){
            echo "Login Success.";
        }else{
            echo "Wrong Credentials.";
        }
    }
} else {
    echo "Username don't exist";
}

$conn-> close();
?>
