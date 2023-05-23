#/usr/bin/bash
export server="localhost"
export port="27017"
export connectionString="mongodb://localhost:27017/"
export database="ItemsDB"
export collectionBefore="ItemsBefore"
export collectionNow="ItemsNow"
export collectionAfter="ItemsAfter"
echo $database $connectionString
dotnet run server="$server" port="$port"

#/  ". ./startup.sh"