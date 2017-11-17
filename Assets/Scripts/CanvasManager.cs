﻿using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine.UI;
using UnityEngine;

public class CanvasManager : MonoBehaviour {
	public Yelp yelp;

	public Text topReview;
	public Text storeName;
	public Image storeFront;
	public Text phoneNumber;
	public Text price;
	public Text address;
	public GameObject panel;

	private string imgUrl = "default";
	private bool flag = true;

	// Use this for initialization
	void Start () {
		flag = true;
		imgUrl = "default";
		reEnableAllStars ();
		panel.SetActive(false);
	}

	private IEnumerator StartImgGet() {
		WWW www = new WWW(imgUrl);
		yield return www;
		storeFront.sprite = Sprite.Create(www.texture, new Rect(0, 0, www.texture.width, www.texture.height), new Vector2(0, 0));
	}

	// Update is called once per frame
	void Update () {
		if (!imgUrl.Equals ("default") && flag) {
			flag = false;
			StartCoroutine(StartImgGet());
		}

		if(Input.GetAxis("XboxA") > 0 && panel.active)
			panel.SetActive(false);
	}

	public void triggerNewFocus(string id)
	{
		//Query for new place information. Display UI. Set new StateManager.focusedId
		panel.SetActive(true);
		reEnableAllStars ();
		flag = true;
		imgUrl = "default";
		yelp.QueryPlaceById(id, delegate(YelpResponse res) {
			topReview.text = res.topReviewAuthor + " - " + res.topReview;
			storeName.text = res.name;
			setRating(res.rating);
			phoneNumber.text = res.phoneNum;
			imgUrl = res.imageUrl;
			if(res.price == null || res.price == "")
				price.text = "Price: N/A";
			else
				price.text = "Price: " + res.price;
			address.text = res.address;

		});
	}

	public void setRating(string _rating)
	{
		//implement rounding
		int rat = (int) Convert.ToDouble (_rating);
		for (int loop = 5; loop > rat; loop--) {
			GameObject.FindGameObjectWithTag ("Star " + loop).GetComponent<Image> ().enabled = false;
		}
	}

	public void reEnableAllStars()
	{
		for (int loop = 5; loop > 0; loop--) {
			GameObject.FindGameObjectWithTag ("Star " + loop).GetComponent<Image> ().enabled = true;
		}
	}

}
