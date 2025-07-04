Feature: Search Artist

To verify the Spotify search artist endpoint work

@tag1
Scenario: Search for a specific artist
	Given I have a valid client credential token
	When I search for the artist "Ed Sheeran"
	Then the response should contain the artist "Ed Sheeran"

