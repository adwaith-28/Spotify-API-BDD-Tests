Feature: Spotify User Profile

To verify the user details endpoint work

@tag1
Scenario: Get current user's profile
	Given I have logged in with Spotify
	When I request my profile
	Then the response should contain my Spotify username
