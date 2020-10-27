# StatisticsAPI

Recommendation: read everything before you start to implement.

Good morning, Agent. 
MI6 is trying to gather statistics about its missions.
Your mission, should you choose to accept it, has two parts:

## Part 1:
##### Endpoint: POST /mission
Body: 
```
{“agent”: “[codename]”, “country”: “[country]”, “address”: “[address string]”, “date”: “[date and time]”}
```
Adds a mission.
See the sample data below.

## Part 2: 
##### Endpoint: GET /countries-by-isolation

An isolated agent is defined as an agent that participated in a single mission.
Implement an algorithm that finds the most isolated country (the country with the highest degree of isolated agents).
For the sample input (see below) input:

* Brazil has 1 isolated agent (008) and 2 non-isolated agents (007, 005)
* Poland has 2 isolated agents (011, 013) and one non-isolated agent (005)
* Morocco has 3 isolated agents (002, 009, 003) and one non-isolated agent (007)

So the result is **Morocco** with an isolation degree of 3.

Part 3:
Find the closest mission from a specific address (any existing mission’s address)

##### Endpoint: POST /find-closest
```
 Body: {“target-location”: “[an address or geo coordinates]”}
```

Note: you can use external API for this (like google)
Your task is to write a service that uses any type of DB (pre-populated with the sample data) and exposes the above endpoints. You can use any framework you see fit.

This message will self-destruct in 2 hours!

Good luck.



Sample data:

```
[
  {agent: '007', country: 'Brazil', 
        address: 'Avenida Vieira Souto 168 Ipanema, Rio de Janeiro',
       date: 'Dec 17, 1995, 9:45:17 PM'
  },
  {agent: '005', country: 'Poland', 
        address: 'Rynek Glowny 12, Krakow',
       date: 'Apr 5, 2011, 5:05:12 PM'
  },
  {agent: '007', country: 'Morocco', 
        address: '27 Derb Lferrane, Marrakech',
       date: 'Jan 1, 2001, 12:00:00 AM'
  },
  {agent: '005', country: 'Brazil', 
        address: 'Rua Roberto Simonsen 122, Sao Paulo',
       date: 'May 5, 1986, 8:40:23 AM'
  },
  {agent: '011', country: 'Poland', 
        address: 'swietego Tomasza 35, Krakow',
       date: 'Sep 7, 1997, 7:12:53 PM'
  },
  {agent: '003', country: 'Morocco', 
        address: 'Rue Al-Aidi Ali Al-Maaroufi, Casablanca',
       date: 'Aug 29, 2012, 10:17:05 AM'
  },
  {agent: '008', country: 'Brazil', 
        address: 'Rua tamoana 418, tefe',
       date: 'Nov 10, 2005, 1:25:13 PM'
  },
  {agent: '013', country: 'Poland', 
        address: 'Zlota 9, Lublin',
       date: 'Oct 17, 2002, 10:52:19 AM'
  },
  {agent: '002', country: 'Morocco', 
        address: 'Riad Sultan 19, Tangier',
       date: 'Jan 1, 2017, 5:00:00 PM'
  },
  {agent: '009', country: 'Morocco', 
        address: 'atlas marina beach, agadir',
       date: 'Dec 1, 2016, 9:21:21 PM'
  }
]

``` 


