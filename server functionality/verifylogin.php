<?php 
ini_set('display_errors', 'On');
error_reporting(E_ALL);

     try {
        $dbh = new PDO('mysql:host=localhost;dbname=dbname;charset=utf8mb4', 'user', 'pw');
    } catch(PDOException $e) {
        echo '<h1>An error has occurred.</h1><pre>', $e->getMessage() ,'</pre>';
    }

$name = $_POST['username'];
$password = $_POST['pw'];

$sql = "SELECT * FROM Users WHERE username = '$name'";
	foreach ($dbh->query($sql) as $data) {
		
		$message=$data['password'];
		$message=$data['ID'];
		$message=$data['username'];
	}
if ($password != '')
{
	if (password_verify($password, $data['password']))
	{
	echo "succes";
    header('LoginSucceed: true');
    header('PlayerID: ' . $data['ID']);
    header('PlayerName: ' . $data['username']);
	}
	else 
	{
	echo "wrong";
    header('LoginSucceed: false');
	}
}
else
{
	echo "wrong";
    header('LoginSucceed: false');
}
$dbh = null;
?>