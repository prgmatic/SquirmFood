using UnityEngine;
using UnityEngine.UI;
using System.Collections.Generic;

public class StarRating : MonoBehaviour
{

    public List<Image> Stars = new List<Image>();
    public Sprite SilverStar;
    public Sprite GoleStar;
    public int Rating { get { return _rating; } }

    private int _rating = 1;

    void Start()
    {
        SetRating(1);
    }

    public void SetRating(int rating)
    {
        _rating = rating;
        for (int i = 0; i < Stars.Count; i++)
        {
            if (i < rating)
            {
                Stars[i].sprite = GoleStar;
            }
            else Stars[i].sprite = SilverStar;
        }
    }
    
}
