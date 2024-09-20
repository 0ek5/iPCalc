using System;
using System.Collections.ObjectModel;
using System.ComponentModel;
using System.Media;
using System.Net;
using System.Printing.IndexedProperties;
using System.Runtime.InteropServices;
using System.Text.RegularExpressions;
using System.Windows;
using System.Windows.Controls;

namespace BinaryToDecimalCalculator
{
    public class CalculatorViewModel : INotifyPropertyChanged
    {
        private string _octed1;
        private string _octed2;
        private string _octed3;
        private string _octed4;

        private string _hex;
        private string _binary;
        private string _decimal;
        private string _ipAddress;
        private bool _isUpdating;

        public string Octed1
        {
            get { return _octed1; }
            set
            {
                if (_octed1 != value)
                {
                    _octed1 = value;
                    OnPropertyChanged(nameof(Octed1));
                    UpdateIPAddress();
                }
            }
        }

        public string Octed2
        {
            get { return _octed2; }
            set
            {
                if (_octed2 != value)
                {
                    _octed2 = value;
                    OnPropertyChanged(nameof(Octed2));
                    UpdateIPAddress();
                }
            }
        }

        public string Octed3
        {
            get { return _octed3; }
            set
            {
                if (_octed3 != value)
                {
                    _octed3 = value;
                    OnPropertyChanged(nameof(Octed3));
                    UpdateIPAddress();
                }
            }
        }

        public string Octed4
        {
            get { return _octed4; }
            set
            {
                if (_octed4 != value)
                {
                    _octed4 = value;
                    OnPropertyChanged(nameof(Octed4));
                    UpdateIPAddress();
                }
            }
        }

        public string IPAddress
        {
            get { return _ipAddress; }
            set
            {
                if (_ipAddress != value)
                {
                    _ipAddress = value;
                    OnPropertyChanged(nameof(IPAddress));
                    ConvertIPAddressToOctets();
                }
            }
        }

        private void UpdateIPAddressFromOctets()
        {
            if (IsValidOctal(Octed1) && IsValidOctal(Octed2) && IsValidOctal(Octed3) && IsValidOctal(Octed4))
            {
                
                int part1 = Convert.ToInt32(Octed1, 8);
                int part2 = Convert.ToInt32(Octed2, 8);
                int part3 = Convert.ToInt32(Octed3, 8);
                int part4 = Convert.ToInt32(Octed4, 8);

                IPAddress = $"{part1}.{part2}.{part3}.{part4}";
            }
            else
            {
                IPAddress = string.Empty;
            }
        }

        private bool IsValidOctal(string value)
        {
            if (string.IsNullOrEmpty(value))
            {
                return false;
            }

            if (!Regex.IsMatch(value, "^[0-7]{1,4}$"))
            {
                return false;
            }

            int decimalValue = Convert.ToInt32(value, 8);
            return decimalValue >= 0 && decimalValue <= 255;
        }

        private void ConvertIPAddressToOctets()
        {
            if (!string.IsNullOrEmpty(IPAddress))
            {
                var parts = IPAddress.Split('.');
                if (parts.Length == 4 &&
                    int.TryParse(parts[0], out int part1) &&
                    int.TryParse(parts[1], out int part2) &&
                    int.TryParse(parts[2], out int part3) &&
                    int.TryParse(parts[3], out int part4))
                {
                    Octed1 = Convert.ToString(part1, 8);
                    Octed2 = Convert.ToString(part2, 8);
                    Octed3 = Convert.ToString(part3, 8);
                    Octed4 = Convert.ToString(part4, 8);
                }
            }
        }

        //public void ConvertFromOcted()
        //{
        //    _isUpdating = true;
        //    try
        //    {
        //        if (!string.IsNullOrEmpty(Octed1) && !string.IsNullOrEmpty(Octed2) && !string.IsNullOrEmpty(Octed3) && !string.IsNullOrEmpty(Octed4))
        //        {
        //            int part1 = Convert.ToInt32(Octed1, 8);
        //            int part2 = Convert.ToInt32(Octed2, 8);
        //            int part3 = Convert.ToInt32(Octed3, 8);
        //            int part4 = Convert.ToInt32(Octed4, 8);

        //            int decimalValue = (part1 << 24) | (part2 << 16) | (part3 << 8) | part4;
        //            Decimal = decimalValue.ToString();
        //            Binary = Convert.ToString(decimalValue, 2);
        //            Hex = decimalValue.ToString("X");
        //        }
        //        else
        //        {
        //            Decimal = string.Empty;
        //            Binary = string.Empty;
        //            Hex = string.Empty;
        //        }
        //    }
        //    catch (Exception ex)
        //    {
        //        Error = $"{ex.Message}";
        //    }
        //    finally
        //    {
        //        _isUpdating = false;
        //    }
        //}



        public string Hex
        {
            get { return _hex; }
            set
            {
                if (_hex != value)
                {
                    if (string.IsNullOrEmpty(value) || Regex.IsMatch(value, "^[0-9A-Fa-f]+$"))
                    {
                        _hex = value;
                        OnPropertyChanged(nameof(Hex));
                        if (!_isUpdating)
                        {
                            ConvertFromHex();
                        }
                    }
                }
            }
        }

        public string Binary
        {
            get { return _binary; }
            set
            {
                if (_binary != value)
                {
                    if (string.IsNullOrEmpty(value) || Regex.IsMatch(value, "^[01]+$"))
                    {
                        _binary = value;
                        OnPropertyChanged(nameof(Binary));
                        if (!_isUpdating)
                        {
                            ConvertFromBinary();
                        }
                    }
                    else
                    {
                        SystemSounds.Beep.Play();
                    }
                }
            }
        }

        public string Decimal
        {
            get { return _decimal; }
            set
            {
                if (_decimal != value)
                {
                    if (string.IsNullOrEmpty(value) || int.TryParse(value, out _))
                    {
                        _decimal = value;
                        OnPropertyChanged(nameof(Decimal));
                        if (!_isUpdating)
                        {
                            ConvertFromDecimal();
                        }
                    }
                }
            }
        }

        private string _error;
        public string Error
        {
            get { return _error; }
            set
            {
                _error = value;
                OnPropertyChanged(nameof(Error));
            }
        }

        private void ConvertFromBinary()
        {
            _isUpdating = true;
            try
            {
                if (!string.IsNullOrEmpty(Binary))
                {
                    uint decimalValue = Convert.ToUInt32(Binary, 2);
                    Decimal = decimalValue.ToString();
                    Hex = decimalValue.ToString("X");
                }
                else
                {
                    Decimal = string.Empty;
                    Hex = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Error = $"{ex.Message}";
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private void ConvertFromDecimal()
        {
            _isUpdating = true;
            try
            {
                if (!string.IsNullOrEmpty(Decimal))
                {
                    uint decimalValue = uint.Parse(Decimal);
                    Binary = Convert.ToString(decimalValue, 2);
                    Hex = decimalValue.ToString("X");
                }
                else
                {
                    Binary = string.Empty;
                    Hex = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Error = $"{ex.Message}";
            }
            finally
            {
                _isUpdating = false;
            }
        }

        private void ConvertFromHex()
        {
            _isUpdating = true;
            try
            {
                if (!string.IsNullOrEmpty(Hex))
                {
                    uint decimalValue = Convert.ToUInt32(Hex, 16);
                    Decimal = decimalValue.ToString();
                    Binary = Convert.ToString(decimalValue, 2);
                }
                else
                {
                    Decimal = string.Empty;
                    Binary = string.Empty;
                }
            }
            catch (Exception ex)
            {
                Error = $"{ex.Message}";
            }
            finally
            {
                _isUpdating = false;
            }
        }

        public void UpdateIPAddress()
        {
            try
            {
                if (!string.IsNullOrEmpty(Octed1) && !string.IsNullOrEmpty(Octed2) && !string.IsNullOrEmpty(Octed3) && !string.IsNullOrEmpty(Octed4))
                {
                    int part1 = Convert.ToInt32(Octed1, 8);
                    int part2 = Convert.ToInt32(Octed2, 8);
                    int part3 = Convert.ToInt32(Octed3, 8);
                    int part4 = Convert.ToInt32(Octed4, 8);

                    IPAddress = $"{part1}.{part2}.{part3}.{part4}";
                }
                else
                {
                    IPAddress = string.Empty;
                }
            }
            catch (Exception ex)
            {   

                Error = $"{ex.Message}";
            }
        }
        

        public void Clear()
        {
            Octed1 = string.Empty;
            Octed2 = string.Empty;
            Octed3 = string.Empty;
            Octed4 = string.Empty;
            IPAddress = string.Empty;
            Decimal = string.Empty;
            Binary = string.Empty;
            Hex = string.Empty;
            Error = string.Empty;
            NetworkAddress = string.Empty;
            BroadcastAddress = string.Empty;
            NetMask = string.Empty;
            FirstUsable = string.Empty;
            LastUsable = string.Empty;
            Cidr = 0;
            NetworkingIP = string.Empty;
            Usable = 0;

        }

        public event PropertyChangedEventHandler PropertyChanged;

        protected virtual void OnPropertyChanged(string propertyName)
        {
            PropertyChanged?.Invoke(this, new PropertyChangedEventArgs(propertyName));
        }







        // IPNetwork
        private string _networkingIP;
        public string NetworkingIP
        {
            get { return _networkingIP; }
            set
            {
                if (_networkingIP != value)
                {
                    _networkingIP = value;
                    IpConverter();
                }
            }
        }

        // IPNetwork Network Address Calculation
  

        public void IpConverter()
        {
            try
            {
                // Parse the actual IP address stored in the IPAddress property
                IPNetwork2 ipnetwork = IPNetwork2.Parse(NetworkingIP);

                // Populate the network details
                NetworkAddress = ipnetwork.Network.ToString();
                BroadcastAddress = ipnetwork.Broadcast.ToString();
                NetMask = ipnetwork.Netmask.ToString();
                FirstUsable = ipnetwork.FirstUsable.ToString();
                LastUsable = ipnetwork.LastUsable.ToString();
                Cidr = ipnetwork.Cidr;
                Usable = (int)ipnetwork.Usable;


            }
            catch (Exception ex)
            {
                Error = $"{ex.Message}";
            }
        }



        

        private string _networkAddress;
        public string NetworkAddress
        {
            get { return _networkAddress; }
            set
            {
                if (_networkAddress != value)
                {
                    _networkAddress = value;
                    OnPropertyChanged(nameof(NetworkAddress));
                }
            }
        }

        private string _netMask;
        public string NetMask
        {
            get { return _netMask; }
            set
            {
                if (_netMask != value)
                {
                    _netMask = value;
                    OnPropertyChanged(nameof(NetMask));
                }
            }
        }

        private string _broadcastAddress;
        public string BroadcastAddress
        {
            get { return _broadcastAddress; }
            set
            {
                if (_broadcastAddress != value)
                {
                    _broadcastAddress = value;
                    OnPropertyChanged(nameof(BroadcastAddress));
                }
            }
        }

        private string _firstUsable;
        public string FirstUsable
        {
            get { return _firstUsable; }
            set
            {
                if (_firstUsable != value)
                {
                    _firstUsable = value;
                    OnPropertyChanged(nameof(FirstUsable));
                }
            }
        }

        private string _lastUsable;
        public string LastUsable
        {
            get { return _lastUsable; }
            set
            {
                if (_lastUsable != value)
                {
                    _lastUsable = value;
                    OnPropertyChanged(nameof(LastUsable));
                }
            }
        }

        private int _cidr;
        public int Cidr
        {
            get { return _cidr; }
            set
            {
                if (_cidr != value)
                {
                    _cidr = value;
                    OnPropertyChanged(nameof(Cidr));
                }
            }
        }

        private int _usable;

        public int Usable
        {
            get { return _usable; }
            set
            {
                if (_usable != value)
                {
                    _usable = value;
                    OnPropertyChanged(nameof(Usable));
                }
            }
        }


    }
}
