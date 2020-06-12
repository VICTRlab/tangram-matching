/*
 * Copyright (C) 2015 Google Inc.
 *
 * Licensed under the Apache License, Version 2.0 (the "License");
 * you may not use this file except in compliance with the License.
 * You may obtain a copy of the License at
 *
 *      http://www.apache.org/licenses/LICENSE-2.0
 *
 * Unless required by applicable law or agreed to in writing, software
 * distributed under the License is distributed on an "AS IS" BASIS,
 * WITHOUT WARRANTIES OR CONDITIONS OF ANY KIND, either express or implied.
 * See the License for the specific language governing permissions and
 * limitations under the License.
 */
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System.Collections;
using GooglePlayGames;
using GooglePlayGames.BasicApi;
using UnityEngine.UI;

// Mainmenu events.
using UnityEngine.SceneManagement;


public class MainMenuEvents : MonoBehaviour
{
	public float fadeSpeed = 1.5f; 
	public Image mFader;
	bool toBlack = false;
	bool toClear = true;
    private Text signInButtonText;
    private Text authStatus;

    void Awake() {
        mFader.color = Color.black;
        mFader.gameObject.SetActive(true);
        toClear = true;
    }

	void Start() {
        GameObject.Find("signInButton").GetComponentInChildren<Text>();
        authStatus = GameObject.Find("authStatus").GetComponent<Text>();
        GameObject startButton = GameObject.Find("startButton");
        EventSystem.current.firstSelectedGameObject = startButton;

        // ADD Play Game Services init code here.
        PlayGamesClientConfiguration config = new
           PlayGamesClientConfiguration.Builder()
           .Build();

        // Enable debugging output (recommended)
        PlayGamesPlatform.DebugLogEnabled = true;

        // Initialize and activate the platform
        PlayGamesPlatform.InitializeInstance(config);
        PlayGamesPlatform.Activate();

        // Try silent sign-in (second parameter is isSilent)
        PlayGamesPlatform.Instance.Authenticate(SignInCallback, true);
    }

    public void SignInCallback(bool success)
    {
        if (success)
        {
            Debug.Log("(Lollygagger) Signed in!");

            // Change sign-in button text
            signInButtonText.text = "Sign out";

            // Show the user's name
            authStatus.text = "Signed in as: " + Social.localUser.userName;
        }
        else
        {
            Debug.Log("(Lollygagger) Sign-in failed...");

            // Show failure message
            signInButtonText.text = "Sign in";
            authStatus.text = "Sign-in failed";
        }
    }

    void LateUpdate()
    {
        if (toBlack) {
            FadeToBlack();
        }
        else if (toClear) {
            FadeToClear();
        }
    }

	public void Play ()
	{
		Debug.Log ("Playing!!");
		toBlack = true;
		// Make sure the texture is enabled.
		mFader.gameObject.SetActive(true);
		
		// Start fading towards black.
		FadeToBlack();

		FadeController fader = gameObject.GetComponentInChildren<FadeController>();
		if (fader != null) {
			fader.FadeToLevel(()=>SceneManager.LoadScene("GameScene"));
		}
		else {
            SceneManager.LoadScene("GameScene");
		}
		

	}

	void FadeToClear ()
	{
		// Lerp the colour of the texture between itself and black.
		mFader.color = Color.Lerp(mFader.color, Color.clear, fadeSpeed * Time.deltaTime);
		// If the screen is almost black...
		if(mFader.color.a <= 0.05f) {

			toClear = false;
			mFader.gameObject.SetActive(false);
			
		}
	}

	void FadeToBlack ()
	{
		// Lerp the colour of the texture between itself and black.
		mFader.color = Color.Lerp(mFader.color, Color.black, fadeSpeed * Time.deltaTime);
		// If the screen is almost black...
		if(mFader.color.a >= 0.95f) {
			// ... reload the level.
			toBlack = false;

		}
	}
    public void SignIn()
    {
        Debug.Log("signInButton clicked!");

        if (!PlayGamesPlatform.Instance.localUser.authenticated)
        {
            // Sign in with Play Game Services, showing the consent dialog
            // by setting the second parameter to isSilent=false.
            PlayGamesPlatform.Instance.Authenticate(SignInCallback, false);
        }
        else
        {
            // Sign out of play games
            PlayGamesPlatform.Instance.SignOut();

            // Reset UI
            signInButtonText.text = "Sign In";
            authStatus.text = "";
        }
    }


    public void changeScene()
    {
        SceneManager.LoadScene(SceneManager.GetActiveScene().buildIndex+1);

    }

}
