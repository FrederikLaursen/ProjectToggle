<?php 
ini_set('display_errors', 'On');
error_reporting(E_ALL);

     try {
        $dbh = new PDO('mysql:host=localhost;dbname=dbname;charset=utf8mb4', 'user', 'pw');
    } catch(PDOException $e) {
        echo '<h1>An error has occurred.</h1><pre>', $e->getMessage() ,'</pre>';
    }

	$id = $_POST['playerID'];
	$score = $_POST['score'];
	$mapSeed = $_POST['mapSeed'];
	$playthrough = $_POST['playthrough'];
	
	$entrychecker = $dbh->query("SELECT count(*) FROM `Highscore` WHERE playerID = '$id' AND mapSeed = '$mapSeed' AND score < '$score'")->fetchColumn();
	echo "$entrychecker";
	if ($entrychecker == 0)
	{
	  $existschecker = $dbh->query("SELECT count(*) FROM `Highscore` WHERE playerID = '$id' AND mapSeed = '$mapSeed'")->fetchColumn();
	
		if ($existschecker == 0)
		{
		$sth = $dbh->prepare("INSERT INTO `Highscore` (playerID,score,mapSeed,playthrough) VALUES (:id,:score,:mapSeed,:playthrough)");	
		$sth->bindParam(':id', $id);
		$sth->bindParam(':score', $score);
		$sth->bindParam(':mapSeed', $mapSeed);
		$sth->bindParam(':playthrough', $playthrough);		
		$sth->execute();
		echo "Creating new instance";
		}
		else
		{
		echo "User already exists";
		}
	}	
	else
	{
	   $updatescore = $dbh->query("UPDATE `Highscore` SET score = '$score' WHERE playerID = '$id' AND mapSeed = '$mapSeed'");
	echo "Updated instance!";
	}
	$dbh = null;
?>