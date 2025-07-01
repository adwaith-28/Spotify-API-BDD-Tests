Feature: Spotify Search Track

To verify the Spotify search endpoint work

@tag1
Scenario: Search for a specific track
	Given I have a valid client credential token
	When I search for the track "Shape Of You"
	Then the response should contain the track "Shape Of You"
