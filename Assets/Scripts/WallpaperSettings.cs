using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace LostPolygon.uLiveWallpaper
{
    public class WallpaperSettings : MonoBehaviour
    {
        static Color lighter = new Color32(81, 8, 91, 255);
        static Color light = new Color32(51, 2, 58, 255);
        static Color dark = new Color32(20, 1, 22, 255);
        public GameObject cover;
        public GameObject spinner;
        private bool moreFeaturesUnlocked = false;

        private void Start()
        {     
            //cover = GameObject.Find("Cover");
           // spinner = GameObject.Find("Spinner");
            SetSpaceColor();
            SetStarDensity();
            SetShootingStarFrequency();
            SetPlanetsFrequency();
            SetTurnSpeed();
            UnlockFeatures();

            SpawnPlanets.Instance.StartSpawn(true, true, true, true);
            SpawnShootingStars.Instance.StartSpawn();
        }
        private void OnEnable()
        {
            LiveWallpaper.PreferenceChanged += LiveWallpaperOnPreferenceChanged;
        }

        private void OnDisable()
        {
            LiveWallpaper.PreferenceChanged -= LiveWallpaperOnPreferenceChanged;
        }

        private void LiveWallpaperOnPreferenceChanged(string key)
        {
            if (key == "general_space_color")
            {
                SetSpaceColor();
            }

            else if (key == "general_star_density")
            {
                SetStarDensity();
            }

            else if (key == "general_shooting_star_frequency")
            {
                SetShootingStarFrequency();
            }

            else if (key == "general_planets_frequency")
            {
                SetPlanetsFrequency();
            }
                                  
            else if (key == "general_turn_speed")
            {
                SetTurnSpeed();
            }
            SpawnPlanets.Instance.StartSpawn(true, true, true, true);
        }

        private void SetSpaceColor()
        {
            string color = LiveWallpaper.Preferences.GetString("general_space_color", "1");
            Color col = light;
            if (color.Equals("0")) col = lighter;
            if (color.Equals("1")) col = light;
            if (color.Equals("2")) col = dark;
            Camera.main.backgroundColor = col;
            cover.GetComponent<SpriteRenderer>().color = col;
        }

        private void SetStarDensity()
        {
            string cycle = LiveWallpaper.Preferences.GetString("general_star_density", "1");
            if (cycle.Equals("0")) SpawnPlanets.Instance.ChangeStarDensity(40);
            if (cycle.Equals("1")) SpawnPlanets.Instance.ChangeStarDensity(60);
            if (cycle.Equals("2")) SpawnPlanets.Instance.ChangeStarDensity(80);
            if (cycle.Equals("3")) SpawnPlanets.Instance.ChangeStarDensity(100);
            if (cycle.Equals("4")) SpawnPlanets.Instance.ChangeStarDensity(120);
        }

        private void SetShootingStarFrequency()
        {
            string cycle = LiveWallpaper.Preferences.GetString("general_shooting_star_frequency", "1");
            if (cycle.Equals("0")) SpawnShootingStars.Instance.ChangeShootingStarFrequency(100, 200);
            if (cycle.Equals("1")) SpawnShootingStars.Instance.ChangeShootingStarFrequency(60, 120);
            if (cycle.Equals("2")) SpawnShootingStars.Instance.ChangeShootingStarFrequency(5, 10);
        }

        private void SetPlanetsFrequency()
        {
            string cycle = LiveWallpaper.Preferences.GetString("general_planets_frequency", "0");
            if (cycle.Equals("0")) SpawnPlanets.Instance.ChangePlanetsFrequency(1, 1, 3);
            if (cycle.Equals("1")) SpawnPlanets.Instance.ChangePlanetsFrequency(1, 2, 6);
            if (cycle.Equals("2")) SpawnPlanets.Instance.ChangePlanetsFrequency(2, 3, 8);
        }

        private void SetTurnSpeed()
        {
            string cycle = LiveWallpaper.Preferences.GetString("general_turn_speed", "2");
            if (cycle.Equals("0")) spinner.GetComponent<CircularMovement>().ChangeTurnSpeed(720);
            if (cycle.Equals("1")) spinner.GetComponent<CircularMovement>().ChangeTurnSpeed(360);
            if (cycle.Equals("2")) spinner.GetComponent<CircularMovement>().ChangeTurnSpeed(180);
            if (cycle.Equals("3")) spinner.GetComponent<CircularMovement>().ChangeTurnSpeed(90);
            if (cycle.Equals("4")) spinner.GetComponent<CircularMovement>().ChangeTurnSpeed(30);
        }

        private void UnlockFeatures()
        {
            string cycle = LiveWallpaper.Preferences.GetString("general_secretKey", "null");
            if (cycle.Equals("hello")) spinner.GetComponent<CircularMovement>().ChangeTurnSpeed(720);
        }
    }
}
