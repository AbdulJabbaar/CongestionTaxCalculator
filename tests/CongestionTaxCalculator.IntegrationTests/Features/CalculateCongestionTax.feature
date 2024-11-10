Feature: CalculateCongestionTax

Want to ensure that toll fees are calculated according to the rule

@tag1
Scenario: Calculate toll feefor multiple times within the same day
	Given a "Regular" vehichle passing through city "Gothenburg" toll stations on "2013-05-15" at following times:
		| Time  |
		| 07:00 |
		| 08:30 |
		| 10:00 |
	When i calculate the toll fee for the day
	Then the total toll fee should be "34"