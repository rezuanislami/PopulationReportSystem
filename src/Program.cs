using System;
using MySql.Data.MySqlClient;

class Program
{
    static void Main()
    {
        // MySQL Database Connection
        string connectionString = "server=localhost;user=root;password=;database=world;";

        // Execute all required queries
        RunQueries(connectionString);
    }

    static void RunQueries(string connectionString)
    {
        using (MySqlConnection conn = new MySqlConnection(connectionString))
        {
            try
            {
                conn.Open();
                Console.WriteLine("Connected to MySQL!\n");

                // ✅ 1. All Countries Ordered by Population
                string query1 = @"SELECT Code, Name, Continent, Region, Population, Capital
                                  FROM country ORDER BY Population DESC;";
                ExecuteQuery(conn, query1, "All Countries Ordered by Population:");

                // ✅ 2. All Cities Ordered by Population
                string query2 = @"SELECT Name, CountryCode, District, Population
                                  FROM city ORDER BY Population DESC;";
                ExecuteQuery(conn, query2, "All Cities Ordered by Population:");

                // ✅ 3. All Capital Cities Ordered by Population
                string query3 = @"SELECT city.Name, country.Name AS Country, city.Population
                                  FROM city
                                  JOIN country ON city.ID = country.Capital
                                  ORDER BY city.Population DESC;";
                ExecuteQuery(conn, query3, "All Capital Cities Ordered by Population:");

                // ✅ 4. Cities in a Country Ordered by Population
                string query4 = @"SELECT city.Name, country.Name AS Country, city.District, city.Population
                                  FROM city
                                  JOIN country ON city.CountryCode = country.Code
                                  WHERE country.Name = 'United States'
                                  ORDER BY city.Population DESC;";
                ExecuteQuery(conn, query4, "Cities in USA Ordered by Population:");

                // ✅ 5. Cities in a District Ordered by Population
                string query5 = @"SELECT Name, CountryCode, District, Population
                                  FROM city WHERE District = 'California'
                                  ORDER BY Population DESC;";
                ExecuteQuery(conn, query5, "Cities in California Ordered by Population:");

                // ✅ 6. Top N Populated Countries (Dynamic Input)
                string query6 = @"SELECT Code, Name, Continent, Region, Population, Capital
                                  FROM country ORDER BY Population DESC LIMIT @N;";
                ExecuteQueryWithLimit(conn, query6, "Top N Most Populated Countries:");

                // ✅ 7. Top N Populated Cities (Dynamic Input)
                string query7 = @"SELECT Name, CountryCode, District, Population
                                  FROM city ORDER BY Population DESC LIMIT @N;";
                ExecuteQueryWithLimit(conn, query7, "Top N Most Populated Cities:");

                // ✅ 8. Population Breakdown by Continent
                string query8 = @"SELECT Continent, SUM(Population) AS TotalPopulation
                                  FROM country GROUP BY Continent ORDER BY TotalPopulation DESC;";
                ExecuteQuery(conn, query8, "Population Breakdown by Continent:");

                // ✅ 9. Population Breakdown by Region
                string query9 = @"SELECT Region, SUM(Population) AS TotalPopulation
                                  FROM country GROUP BY Region ORDER BY TotalPopulation DESC;";
                ExecuteQuery(conn, query9, "Population Breakdown by Region:");

                // ✅ 10. Language Speaker Statistics
                string query10 = @"SELECT Language, SUM(Population * Percentage / 100) AS Speakers
                                   FROM countrylanguage
                                   JOIN country ON countrylanguage.CountryCode = country.Code
                                   WHERE Language IN ('Chinese', 'English', 'Hindi', 'Spanish', 'Arabic')
                                   GROUP BY Language ORDER BY Speakers DESC;";
                ExecuteQuery(conn, query10, "Language Speaker Statistics:");

                // ✅ 11. Population Breakdown by District (FIXED & COMPLETE)
                string query11 = @"
                    SELECT city.District,
                           SUM(city.Population) AS CityPopulation,
                           (SELECT SUM(country.Population)
                            FROM country
                            WHERE country.Code = city.CountryCode) - SUM(city.Population) AS NonCityPopulation,
                           ROUND(SUM(city.Population) /
                                 (SELECT SUM(country.Population)
                                  FROM country
                                  WHERE country.Code = city.CountryCode) * 100, 2) AS CityPercentage
                    FROM city
                    JOIN country ON city.CountryCode = country.Code
                    GROUP BY city.District
                    ORDER BY CityPopulation DESC;";
                ExecuteQuery(conn, query11, "Population Breakdown by District (City vs Non-City Population):");

            }
            catch (Exception ex)
            {
                Console.WriteLine("Error: " + ex.Message);
            }
        }
    }

    // Method to execute queries and display results
    static void ExecuteQuery(MySqlConnection conn, string query, string message)
    {
        Console.WriteLine("\n" + message);
        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + " | ");
                    }
                    Console.WriteLine();
                }
            }
        }
        Console.WriteLine();
    }

    // Method to execute queries with a dynamic limit (Top N)
    static void ExecuteQueryWithLimit(MySqlConnection conn, string query, string message)
    {
        Console.Write("\n" + message + "\nEnter the value for N: ");
        int n = int.Parse(Console.ReadLine());

        Console.WriteLine();
        using (MySqlCommand cmd = new MySqlCommand(query, conn))
        {
            cmd.Parameters.AddWithValue("@N", n);
            using (MySqlDataReader reader = cmd.ExecuteReader())
            {
                while (reader.Read())
                {
                    for (int i = 0; i < reader.FieldCount; i++)
                    {
                        Console.Write(reader[i] + " | ");
                    }
                    Console.WriteLine();
                }
            }
        }
        Console.WriteLine();
    }
}

