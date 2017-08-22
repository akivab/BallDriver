using System.Collections;
using System.Collections.Generic;
using UnityEngine;


public enum LatestNetworkUpdate {
	IDLE,
	LOADING_IMAGE,
	DOWNLOADED_IMAGE
};

public class LocalNetworkScanner {
	private bool isDownloading = false;
	private WWW theWWW = null;

	private Texture2D latestImageTexture = null;
	private string latestUpdateString = null;

	private LatestNetworkUpdate latestNetworkUpdate = LatestNetworkUpdate.IDLE;
	float elapsedTime = 0.0f;

	private string host = "http://localhost:8080";
	
	private string lastUpdatePath = "/last_command";
	private string lastImagePath = "/last_image";

	private int kMaxWaitTimeInSeconds = 10;

	public void ResetState() {
		latestNetworkUpdate = LatestNetworkUpdate.IDLE;
		theWWW.Dispose ();
		isDownloading = false;
	}
	public IEnumerator Download() {
		isDownloading = true;
		elapsedTime = 0.0f;
		string urlToQuery = host;
		if (latestNetworkUpdate == LatestNetworkUpdate.IDLE) {
			urlToQuery += lastUpdatePath;
		} else if (latestNetworkUpdate == LatestNetworkUpdate.LOADING_IMAGE) {
			urlToQuery += lastImagePath;
		} else {
			yield break;
		}

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
				if (latestUpdateString.Contains ("image")) {
					latestNetworkUpdate = LatestNetworkUpdate.LOADING_IMAGE;
				} else {
					latestNetworkUpdate = LatestNetworkUpdate.IDLE;
				}
			}
		}
		isDownloading = false;
	}

	public Texture2D getLatestImageTexture() {
		Debug.Log ("image of type: " + latestImageTexture.GetType());
		return latestImageTexture;
	}

	public LatestNetworkUpdate getLatestNetworkUpdate() {
		return latestNetworkUpdate;
	}

	public bool ShouldStartDownload () {
		if (isDownloading) {
			elapsedTime += Time.deltaTime;
			if (elapsedTime > kMaxWaitTimeInSeconds && !theWWW.isDone) {
				Debug.Log ("Cancelling due to timeout.");
				ResetState ();
			}
		}
		return !isDownloading;
	}
}
