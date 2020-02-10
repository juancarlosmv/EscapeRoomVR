using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class VRKeypadController : VRButtonController
{
    private char[] screenCode;
    private int ind;
    [SerializeField]
    private Text text;
    [SerializeField]
    private int pass;
    private int numDigits;
    [SerializeField]
    private KeyOkLed led;

    private void Start()
    {
        numDigits = (int)Mathf.Ceil(Mathf.Log10(pass));
        screenCode = new char[numDigits];
        ResetCode();
        ind = 0;
        UpdateScreen();
    }

    public override void ProcessPush(char code)
    {
        if (code >= '0' && code <= '9') AddDigit(code);
        else if (code == 'c') ResetCode();
        else if (code == 'e') CheckOk();
        UpdateScreen();
    }

    public void AddDigit(char digit)
    {
        if (ind < numDigits)
        {
            ShiftLeft();
            screenCode[numDigits - 1] = digit;
            ind++;
        }
    }

    public void ResetCode()
    {
        for (int i = 0; i < numDigits; i++) screenCode[i] = '_';
        ind = 0;
        // Change when final application is decided
        led.SetBase();
    }

    public void CheckOk()
    {
        bool ok = true;
        int num = pass;

        // Dont check if not full
        if (ind < numDigits) return;
        for(int i=0; i< numDigits; i++)
        {
            if(screenCode[numDigits - i - 1] - '0' != num % 10)
            {
                ok = false;
                break;
            }
            num /= 10;
        }

        // Change when final application is decided
        if (ok) led.SetOk();
        else led.SetError();
    }

    public void UpdateScreen()
    {
        text.text = "";
        for (int i = 0; i < numDigits; i++) text.text += screenCode[i];
    }

    private void ShiftLeft()
    {
        for (int i = 0; i < numDigits - 1; i++) screenCode[i] = screenCode[i + 1];
        screenCode[numDigits - 1] = '_';
    }
}
