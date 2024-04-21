<?php
include 'cors.php';

$imageName=$_GET['image'];

$path = "affiches/".$imageName;

$image = file_get_contents($path);

echo $image;

?>
