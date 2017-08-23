using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LatestNetworkUpdate {
	IDLE,
	LOADING_IMAGE,
	DOWNLOADED_IMAGE,
	GOT_LATEST_COMMAND
};

public class LocalNetworkScanner {
	private bool isDownloading = false;
	private WWW theWWW = null;

	private Texture2D latestImageTexture = null;
	private string latestUpdateString = null;
	private string latestAction = "";
	private char latestCommand = '\0';

	private LatestNetworkUpdate latestNetworkUpdate = LatestNetworkUpdate.IDLE;
	float elapsedTime = 0.0f;
	float waitTime = 0.0f;
	private string hostIp = "127.0.0.1"; //"10.29.6.73";
	private string port = "8080";
	private string host;
	private string lastUpdatePath = "/lastCommand";
	private string lastImagePath = "/lastImage";

	private int kMaxWaitTimeInSeconds = 10;
	private int kMinWaitTimeInSeconds = 5;

	public LocalNetworkScanner() {
		host = "http://" + hostIp + ":" + port;
		Debug.Log("host: " + host);
	}

	public void ResetState() {		
	 	latestNetworkUpdate = LatestNetworkUpdate.IDLE;
		theWWW.Dispose ();
		theWWW = null;
		isDownloading = false;
	}
	private void handleUpdatedCommand(string lastCommand, string lastAction) {
		if (lastCommand.Equals ("image")) {
			latestNetworkUpdate = LatestNetworkUpdate.LOADING_IMAGE;
		} else if (lastCommand.Trim().Length == 1 && lastCommand.ToCharArray()[0] >= 'A' && lastCommand.ToCharArray()[0] <= 'Z') {
			latestNetworkUpdate = LatestNetworkUpdate.GOT_LATEST_COMMAND;
			latestAction = lastAction;
			latestCommand = latestUpdateString.ToCharArray()[0];
		} else {
			latestNetworkUpdate = LatestNetworkUpdate.IDLE;
		}
	}

	public IEnumerator Download() {
		if (latestNetworkUpdate != LatestNetworkUpdate.IDLE && latestNetworkUpdate != LatestNetworkUpdate.LOADING_IMAGE)
		{
			yield break;
		}
		string urlToQuery = host;

		if (latestNetworkUpdate == LatestNetworkUpdate.IDLE) {
			urlToQuery += lastUpdatePath;
		} else if (latestNetworkUpdate == LatestNetworkUpdate.LOADING_IMAGE) {
			urlToQuery += lastImagePath;
		} else {
			Debug.Log("No need for downloading, returning!");
			yield break;
		}

		isDownloading = true;
		elapsedTime = 0.0f;

		Debug.Log("Looking for WWWW");
		theWWW = new WWW(urlToQuery);
		yield return theWWW;

		if (theWWW.error != null) {
			Debug.Log ("saw error: " + theWWW.error);
			ResetState ();
			yield break;
		}
		byte[] bytes = theWWW.bytes;

		if (latestNetworkUpdate == LatestNetworkUpdate.LOADING_IMAGE) {
			latestImageTexture = theWWW.texture;
			latestNetworkUpdate = LatestNetworkUpdate.DOWNLOADED_IMAGE;
		} else if (latestNetworkUpdate == LatestNetworkUpdate.IDLE) {
			string currentString = System.Text.Encoding.UTF8.GetString (bytes);
			if (!currentString.Equals (latestUpdateString)) {
				Debug.Log ("from " + latestUpdateString + " to " + currentString);
				latestUpdateString = currentString;
				if (latestUpdateString.IndexOf(",") == -1)
				{
					latestNetworkUpdate = LatestNetworkUpdate.IDLE;
				}
				else
				{
					string[] parts = latestUpdateString.Split(',');
					if (parts.Length != 3)
					{
						latestNetworkUpdate = LatestNetworkUpdate.IDLE;
					}
					else
					{
						handleUpdatedCommand(parts[0], parts[1]);
					}
				}
			}
		}
		isDownloading = false;
	}

	public Texture2D getLatestImageTexture() {
		return latestImageTexture;
	}

	public LatestNetworkUpdate getLatestNetworkUpdate() {
		return latestNetworkUpdate;
	}

	public char getLatestCommand() {
		return latestCommand;
	}

	public string getLatestAction() {
		return latestAction;
	}

	public bool ShouldStartDownload () {
		if (isDownloading)
		{
			elapsedTime += Time.deltaTime;
			if (elapsedTime > kMaxWaitTimeInSeconds && theWWW != null && !theWWW.isDone)
			{
				Debug.Log("Cancelling due to timeout.");
				ResetState();
			}
		}
		else if (latestNetworkUpdate == LatestNetworkUpdate.IDLE && waitTime < kMinWaitTimeInSeconds)
		{
			waitTime += Time.deltaTime;
			return false;
		}
		else if (latestNetworkUpdate != LatestNetworkUpdate.IDLE && latestNetworkUpdate != LatestNetworkUpdate.LOADING_IMAGE)
		{
			return false;
		}
		waitTime = 0;
		return !isDownloading;
	}
}
