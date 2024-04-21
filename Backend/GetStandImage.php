<?php
include 'cors.php';

$imageName=$_GET['image'];

$path = "stands/".$imageName;

$image = file_get_contents($path);

echo $image;

?>
