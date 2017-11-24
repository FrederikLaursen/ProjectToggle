<?php
ini_set('display_errors', 'On');
error_reporting(E_ALL);

$servername = "localhost";
$username = "username";
$password = "pw";
$dbname = "dbname";

	// Create connection
	$conn = new PDO("mysql:host=$servername;dbname=$dbname", $username, $password);
	
	$sql = 'SELECT * FROM Highscore ORDER BY score DESC';
	
	foreach ($conn->query($sql) as $data) {
		echo json_encode(array(
		'score' => $data['score'],
		'mapSeed' => $data['mapSeed'],
		'playerId' => $data['playerID'],
		'playThrough' => $data['playthrough'],
		));
	}

	$conn = null;
?>
