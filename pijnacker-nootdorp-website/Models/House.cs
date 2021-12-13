﻿using Newtonsoft.Json;
using System.Collections.Generic;

public class House
{
    public enum Accessibility { Wheelchair, Car, PublicTransport }

    public class AccessModel
    {
        public bool wheelchair;
        public bool car;
        public bool publicTransport;
    }

    public int Id { get; set; }

    public string Address { get; set; }
    public int Price { get; set; }
    public string Description { get; set; }

    public int Access { get; set; }

    public int BuildYear { get; set; }

    public float OutdoorArea { get; set; }
    public float IndoorArea { get; set; }

    public string Rooms { get; set; }
    public string Picture { get; set; }

    private HouseLayout _layout = null;
    public HouseLayout Layout
    {
        get
        {
            if (_layout == null)
            {
                try
                {
                    _layout = JsonConvert.DeserializeObject<HouseLayout>(Rooms);
                }
                catch
                {
                    _layout = new HouseLayout();
                }
            }

            return _layout;
        }
    }

    private AccessModel _accessData = null;
    public AccessModel AccessData
    {
        get
        {
            if (_accessData == null)
            {
                _accessData = new AccessModel();

                List<int> digits = NumberUtilities.GetDigits(Access);
                digits.Reverse();

                _accessData.wheelchair = digits[0] == 1;
                if (digits.Count > 1) _accessData.car = digits[1] == 1;
                if (digits.Count > 2) _accessData.publicTransport = digits[2] == 1;
            }

            return _accessData;
        }
    }
}