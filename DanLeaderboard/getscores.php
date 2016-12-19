<?php
header('Access-Control-Allow-Origin: *');

//Details of database, for connection
$host="localhost"; //Host name 
$username="chessgam_dan"; //Mysql username 
$password="q1w2e3r4"; //Mysql password 
$db_name="chessgam_leaderboard"; //Database name 
$tbl_name="scores"; //Table name

//Connect to server and select database
mysql_connect("$host", "$username", "$password")or die("cannot connect"); 
mysql_select_db("$db_name")or die("cannot select DB");

//Retrieve data from database 
$sql="SELECT * FROM scores ORDER BY rating DESC LIMIT 100";//Order by rating
$result=mysql_query($sql);//Store result in variable

//Start looping rows in mysql database
while($rows=mysql_fetch_array($result)){
echo $rows['name'] . " ----- " . $rows['rating'] . " ---------- " . $rows['time'] . "<br />\n";//Echo name, rating and time then break and new line
}

//Close MySQL connection 
mysql_close();
?>