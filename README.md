# Spotify API BDD Tests

In this project I implemented BDD style API testing using Spotify's Web APIs. The tests are written in **C#**, use **SpecFlow** for BDD, **NUnit** for assertions, and interact with Spotify via both **Client Credentials Flow** and **Authorization Code Flow (OAuth2)**. I had to learn the different types of authentication, particularly oAuth 2.0 because Spotify uses that for authentication. This was more challenging that I expected, but enjoyed it regardless :) 

## Features Covered

### 1. Search Track (Public API)
=> Uses Spotify's `/v1/search` endpoint. <br />
=> Searches for a track by name. <br />
=> Verifies that the response contains the expected track. <br />

<img width="675" alt="{BA1A224E-993D-4A63-BC51-F9B4C18ABC03}" src="https://github.com/user-attachments/assets/050be30c-36c7-4d43-9e27-5fddf0d12f02" />

### 2. Search Artist (Public API)
=> Uses Spotify's `/v1/search` endpoint. <br />
=> Searches for an artist by name. <br />
=> Asserts the artist appears in the response. <br />

<img width="816" alt="{8205C7CD-7617-4312-A5B0-A6B609F04058}" src="https://github.com/user-attachments/assets/9f64f33d-7361-43b7-a32d-c0975a5346dc" />

### 3. Get My Profile (Private API with OAuth2)
=> Implements **Authorization Code Flow** to get user-level access. <br />
=> Launches a browser for user login and consent. <br />
=> Starts a local HTTP listener to receive the `code`. <br />
=> Exchanges the code for an `access_token` and fetches `/v1/me` profile. <br />

<img width="812" alt="{E1FBA1A3-077B-4E96-9220-579EFC699991}" src="https://github.com/user-attachments/assets/fc683288-3c37-4aac-aee2-f3ea87af8a25" />


