SELECT Code, Name, Continent, Region, Population, Capital
FROM Country
ORDER BY Population DESC;
SELECT Name, Country, District, Population
FROM City
ORDER BY Population DESC;
SELECT Name, Country, Population
FROM City
WHERE IsCapital = 1  -- Assuming there's a column indicating capital cities
ORDER BY Population DESC;
SELECT Code, Name, Continent, Region, Population, Capital
FROM Country
ORDER BY Population DESC
LIMIT @N;
SELECT Name, Country, District, Population
FROM City
ORDER BY Population DESC
LIMIT @N;
SELECT Continent, SUM(Population) AS TotalPopulation
FROM Country
GROUP BY Continent;
SELECT Continent,
       SUM(CASE WHEN City.Population > 0 THEN City.Population ELSE 0 END) AS CityPopulation,
       SUM(CASE WHEN City.Population = 0 THEN Country.Population ELSE 0 END) AS NonCityPopulation
FROM Country
LEFT JOIN City ON Country.Code = City.CountryCode
GROUP BY Continent;
SELECT Language, SUM(Population) AS Speakers
FROM Country
JOIN CountryLanguage ON Country.Code = CountryLanguage.CountryCode
WHERE CountryLanguage.Language IN ('Chinese', 'English', 'Hindi', 'Spanish', 'Arabic')
GROUP BY Language
ORDER BY Speakers DESC;
SELECT SUM(Population) AS WorldPopulation
FROM Country;
SELECT SUM(Population) AS ContinentPopulation
FROM Country
WHERE Continent = @Continent;
SELECT SUM(Population) AS RegionPopulation
FROM Country
WHERE Region = @Region;
SELECT SUM(Population) AS CountryPopulation
FROM City
WHERE CountryCode = @CountryCode;
SELECT SUM(Population) AS DistrictPopulation
FROM City
WHERE District = @District;
SELECT Population
FROM City
WHERE Name = @CityName;
