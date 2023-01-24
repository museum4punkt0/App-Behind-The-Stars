#region Description

/*Sämtliche Texte des Interface werden in dieser Klasse übersetzt. Dafür sind alle Texte und Sprites im Inspector
 * den public Variablen zugewiesen.
 */

#endregion

using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;

public class Translator : MonoBehaviour {
    public TextMeshProUGUI weiterButton;
    public Text weiterButtonUIText;
    public TextMeshProUGUI hilfeButton;
    public Text hilfeButtonUIText;

    public TextMeshProUGUI burgerMenuPfadBeenden;
    public TextMeshProUGUI burgerMenuKommentar;
    public TextMeshProUGUI burgerMenuLogfile;
    public TextMeshProUGUI burgerMenuPhilosophie;
    public TextMeshProUGUI burgerMenuImpressum;
    public TextMeshProUGUI burgerMenuBeenden;
    public TextMeshProUGUI burgerMenuMenuschliesen;

    public Text checkPointJa;
    public Text checkPointNein;

    public Text jumpBackQuestionText;
    public Text jumpBackQuestionJaButton;
    public TextMeshPro sommerTextS02;

    public TextMeshProUGUI finishGratulation;
    public TextMeshProUGUI finishButton;
    public TextMeshProUGUI finishState;
    public TextMeshProUGUI finishInfo;

    public Sprite n09OverviewDE;
    public Sprite n09OverviewEN;
    public Image n09overview;

    public TextMeshProUGUI weekDayScrollCalendarMontag;
    public TextMeshProUGUI weekDayScrollCalendarDienstag;
    public TextMeshProUGUI weekDayScrollCalendarMittwoch;
    public TextMeshProUGUI weekDayScrollCalendarDonnerstag;
    public TextMeshProUGUI weekDayScrollCalendarFreitag;
    public TextMeshProUGUI weekDayScrollCalendarSamstag;
    public TextMeshProUGUI weekDayScrollCalendarSonntag;

    public TextMeshProUGUI monthScrollCalendar01;
    public TextMeshProUGUI monthScrollCalendar02;
    public TextMeshProUGUI monthScrollCalendar03;
    public TextMeshProUGUI monthScrollCalendar04;
    public TextMeshProUGUI monthScrollCalendar05;
    public TextMeshProUGUI monthScrollCalendar06;
    public TextMeshProUGUI monthScrollCalendar07;
    public TextMeshProUGUI monthScrollCalendar08;
    public TextMeshProUGUI monthScrollCalendar09;
    public TextMeshProUGUI monthScrollCalendar10;
    public TextMeshProUGUI monthScrollCalendar11;
    public TextMeshProUGUI monthScrollCalendar12;
    public TextMeshProUGUI monthScrollCalendar13;
    public TextMeshProUGUI monthScrollCalendar14;
    public TextMeshProUGUI monthScrollCalendar15;
    public TextMeshProUGUI monthScrollCalendar16;
    public TextMeshProUGUI monthScrollCalendar17;

    public TextMeshProUGUI buttonBestaetigen;
    public Text buttonBestaetigen2;

    public Text clockScroll00;
    public Text clockScroll01;
    public Text clockScroll02;
    public Text clockScroll03;
    public Text clockScroll04;
    public Text clockScroll05;
    public Text clockScroll06;
    public Text clockScroll07;
    public Text clockScroll08;
    public Text clockScroll09;
    public Text clockScroll10;
    public Text clockScroll11;
    public Text clockScroll12;
    public Text clockScroll13;
    public Text clockScroll14;
    public Text clockScroll15;
    public Text clockScroll16;
    public Text clockScroll17;
    public Text clockScroll18;
    public Text clockScroll19;
    public Text clockScroll20;
    public Text clockScroll21;
    public Text clockScroll22;
    public Text clockScroll23;
    public TextMeshProUGUI clockFormat;

    public TextMeshProUGUI headline;
    public TextMeshProUGUI subHeadline;
    public Text subHeadlineUIText;
    public CheckPointController cPC;

    public GameObject impressum;
    public GameObject impressumEN;
    public GameObject impressum2Down;
    public GameObject impressumEN2Down;

    public GameObject phiolosphie;
    public GameObject phiolosphieEN;

    public TextMeshProUGUI impressumHeadline;
    public TextMeshProUGUI philosophieHeadline;

    public TextMeshPro groundOsten;
    public TextMeshProUGUI dateText;
    public TextMeshProUGUI timeText;

    public TextMeshProUGUI moonDay01;
    public TextMeshProUGUI moonDay02;
    public TextMeshProUGUI moonDay03;
    public TextMeshProUGUI moonDay04;
    public TextMeshProUGUI moonDay05;
    public TextMeshProUGUI moonDay06;
    public TextMeshProUGUI moonDay07;

    public GameObject weekCalendarContentParent;
    
    public Sprite tippHighlight_DE;
    public Sprite tippHighlight_EN;
    public Image tippHighlight;
    public Text jumpBackCechpointMenu;
    public void ChangeLanguage(int language, string aktStep, ProcedureController.BubbleList myBubbleList) {
        if (language == 0) {
            weiterButton.text = "WEITER";
            hilfeButton.text = "TIPP";
            weiterButtonUIText.text = "WEITER";
            hilfeButtonUIText.text = "TIPP";

            burgerMenuPfadBeenden.text = "PFAD BEENDEN";
            burgerMenuKommentar.text = "SCHRITT KOMMENTIEREN";
            burgerMenuLogfile.text = "LOGFILE ÜBERMITTELN";
            burgerMenuPhilosophie.text = "UNSERE PHILOSOPHIE";
            burgerMenuImpressum.text = "IMPRESSUM";
            burgerMenuBeenden.text = "APP BEENDEN";
            burgerMenuMenuschliesen.text = "APP BEENDEN";

            checkPointJa.text = "JA";
            checkPointNein.text = "NEIN";

            jumpBackQuestionText.text = "Zurück zum letzten Checkpoint?";
            jumpBackQuestionJaButton.text = "JA";
            sommerTextS02.text = "SOMMER";
            finishGratulation.text = "GRATULATION!";
            finishButton.text = "WEITER";
            n09overview.sprite = n09OverviewDE;

            weekDayScrollCalendarMontag.text = "Montag";
            weekDayScrollCalendarDienstag.text = "Dienstag";
            weekDayScrollCalendarMittwoch.text = "Mittwoch";
            weekDayScrollCalendarDonnerstag.text = "Donnerstag";
            weekDayScrollCalendarFreitag.text = "Freitag";
            weekDayScrollCalendarSamstag.text = "Samstag";
            weekDayScrollCalendarSonntag.text = "Sonntag";

            monthScrollCalendar01.text = "Oktober";
            monthScrollCalendar02.text = "November";
            monthScrollCalendar03.text = "Dezember";
            monthScrollCalendar04.text = "Januar";
            monthScrollCalendar05.text = "Februar";
            monthScrollCalendar06.text = "März";
            monthScrollCalendar07.text = "April";
            monthScrollCalendar08.text = "Mai";
            monthScrollCalendar09.text = "Juni";
            monthScrollCalendar10.text = "Juli";
            monthScrollCalendar11.text = "August";
            monthScrollCalendar12.text = "September";
            monthScrollCalendar13.text = "Oktober";
            monthScrollCalendar14.text = "November";
            monthScrollCalendar15.text = "Dezember";
            monthScrollCalendar16.text = "Januar";
            monthScrollCalendar17.text = "Februar";

            clockScroll00.text = "0";
            clockScroll01.text = "1";
            clockScroll02.text = "2";
            clockScroll03.text = "3";
            clockScroll04.text = "4";
            clockScroll05.text = "5";
            clockScroll06.text = "6";
            clockScroll07.text = "7";
            clockScroll08.text = "8";
            clockScroll09.text = "9";
            clockScroll10.text = "10";
            clockScroll11.text = "11";
            clockScroll12.text = "12";
            clockScroll13.text = "13";
            clockScroll14.text = "14";
            clockScroll15.text = "15";
            clockScroll16.text = "16";
            clockScroll17.text = "17";
            clockScroll18.text = "18";
            clockScroll19.text = "19";
            clockScroll20.text = "20";
            clockScroll21.text = "21";
            clockScroll22.text = "22";
            clockScroll23.text = "23";
            clockFormat.text = "UHR";

            buttonBestaetigen.text = "BESTÄTIGEN";
            buttonBestaetigen2.text = "BESTÄTIGEN";

            impressum.SetActive(true);
            impressumEN.SetActive(false);  
            impressum2Down.SetActive(true);
            impressumEN2Down.SetActive(false);
            impressumHeadline.text = "IMPRESSUM";
            philosophieHeadline.text = "PHILOSOPHIE";
            phiolosphie.SetActive(true);
            phiolosphieEN.SetActive(false);
            groundOsten.text = "O";

            if (aktStep.Contains("N01.") || aktStep.Contains("N02.") || aktStep.Contains("N03.") ||
                aktStep.Contains("N05.") || aktStep.Contains("N07.") || aktStep.Contains("N08.") ||
                aktStep.Contains("N019.")) {
                headline.text = "NOKTURNAL";
            }

            if (aktStep.Contains("Z01.") || aktStep.Contains("S01.") || aktStep.Contains("S02.") ||
                aktStep.Contains("S03.") || aktStep.Contains("S05.")) {
                headline.text = "SONNENUHR";
            }


            if (aktStep.Contains("Z02.") || aktStep.Contains("H0.")) {
                headline.text = "HIMMELSGLOBUS";
            }

            string aktmonth = GetMonthNumber(dateText.text);
            string[] arr = dateText.text.Split(new string[] { " " }, StringSplitOptions.None);
            string[] arr1 = arr[1].Split(new string[] { "," }, StringSplitOptions.None);
            dateText.text = arr1[0] + "." + aktmonth + "." + System.DateTime.Today.Year;
            
            string[] arrtimeText = timeText.text.Split(new string[] { " " }, StringSplitOptions.None);
            
            if (timeText.text.Contains("pm")) {
                string[] arrtimeText2 = arrtimeText[0].Split(new string[] { ":" }, StringSplitOptions.None);
                int currentHour = int.Parse(arrtimeText2[0])+12;
                timeText.text = currentHour.ToString() + ":" + arrtimeText2[1];
            } else if (timeText.text.Contains("Noon")) {
                timeText.text = "Mittag";
            }else{
                timeText.text = arrtimeText[0];
            }

            if (finishState.text.Contains("nocturnal")&& finishState.text.Length>2) {
                string[] arrFinishState = finishState.text.Split(new string[] { "You've completed" }, StringSplitOptions.None);
                string[] arrFinishState2 = arrFinishState[1].Split(new string[] { " of 5" }, StringSplitOptions.None);
                finishState.text="Du hast " + arrFinishState2[0] + "/5 Pfade des Nokturnals erfolgreich absolviert.";
            }else if (finishState.text.Contains("sundial") && finishState.text.Length > 2) {
                string[] arrFinishState = finishState.text.Split(new string[] { "You've completed" }, StringSplitOptions.None);
                string[] arrFinishState2 = arrFinishState[1].Split(new string[] { " of 5" }, StringSplitOptions.None);
                finishState.text="Du hast " + arrFinishState2[0] + "/5 Pfade der Sonnenuhr erfolgreich absolviert.";
            }
            moonDay01.text = "+1 Tag";
            moonDay02.text = "+2 Tage";
            moonDay03.text = "+3 Tage";
            moonDay04.text = "+4 Tage";
            moonDay05.text = "+5 Tage";
            moonDay06.text = "+6 Tage";
            moonDay07.text = "+7 Tage";
            
            foreach (Transform child in weekCalendarContentParent.transform) {
                string monthname = child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text;
                if (monthname.Contains("MAY")) {
                    child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "MAI.";
                }
                if (monthname.Contains("OCT")) {
                    child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "OKT.";
                }
                if (monthname.Contains("DEC")) {
                    child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "DEZ.";
                }
            }
            tippHighlight.sprite = tippHighlight_DE;
            jumpBackCechpointMenu.text = "MENÜ";
        }

        if (language == 1) {
            weiterButton.text = "NEXT";
            hilfeButton.text = "TIP";
            weiterButtonUIText.text = "NEXT";
            hilfeButtonUIText.text = "TIP";

            burgerMenuPfadBeenden.text = "BACK TO MAIN MENU";
            burgerMenuKommentar.text = "COMMENT ON THIS STEP";
            burgerMenuLogfile.text = "SEND LOGFILE";
            burgerMenuPhilosophie.text = "OUR PHILOSOPHY";
            burgerMenuImpressum.text = "CREDITS";
            burgerMenuBeenden.text = "QUIT THE APP";
            burgerMenuMenuschliesen.text = "QUIT THE APP";

            checkPointJa.text = "YES";
            checkPointNein.text = "NO";

            jumpBackQuestionText.text = "BACK TO LAST CHECKPOINT?";
            jumpBackQuestionJaButton.text = "OK";
            //jumpBackQuestionAbbruchButton.text = "CANCEL";
            //jumpBackQuestionAndererCPButton.text = "OTHER CHECKPOINT";
            sommerTextS02.text = "SUMMER";

            finishGratulation.text = "CONGRATULATIONS!";
            finishButton.text = "NEXT";

            n09overview.sprite = n09OverviewEN;

            weekDayScrollCalendarMontag.text = "Monday";
            weekDayScrollCalendarDienstag.text = "Tuesday";
            weekDayScrollCalendarMittwoch.text = "Wednesday";
            weekDayScrollCalendarDonnerstag.text = "Thursday";
            weekDayScrollCalendarFreitag.text = "Friday";
            weekDayScrollCalendarSamstag.text = "Saturday";
            weekDayScrollCalendarSonntag.text = "Sunday";

            monthScrollCalendar01.text = "October";
            monthScrollCalendar02.text = "November";
            monthScrollCalendar03.text = "December";
            monthScrollCalendar04.text = "January";
            monthScrollCalendar05.text = "February";
            monthScrollCalendar06.text = "March";
            monthScrollCalendar07.text = "April";
            monthScrollCalendar08.text = "May";
            monthScrollCalendar09.text = "June";
            monthScrollCalendar10.text = "July";
            monthScrollCalendar11.text = "August";
            monthScrollCalendar12.text = "September";
            monthScrollCalendar13.text = "October";
            monthScrollCalendar14.text = "November";
            monthScrollCalendar15.text = "December";
            monthScrollCalendar16.text = "January";
            monthScrollCalendar17.text = "February";

            clockScroll00.text = "1";
            clockScroll01.text = "2";
            clockScroll02.text = "3";
            clockScroll03.text = "4";
            clockScroll04.text = "5";
            clockScroll05.text = "6";
            clockScroll06.text = "7";
            clockScroll07.text = "8";
            clockScroll08.text = "9";
            clockScroll09.text = "10";
            clockScroll10.text = "11";
            clockScroll11.text = "12";
            clockScroll12.text = "1";
            clockScroll13.text = "2";
            clockScroll14.text = "3";
            clockScroll15.text = "4";
            clockScroll16.text = "5";
            clockScroll17.text = "6";
            clockScroll18.text = "7";
            clockScroll19.text = "8";
            clockScroll20.text = "9";
            clockScroll21.text = "10";
            clockScroll22.text = "11";
            clockScroll23.text = "12";
            clockFormat.text = "pm";

            buttonBestaetigen.text = "SELECT";
            buttonBestaetigen2.text = "SELECT";

            impressum.SetActive(false);
            impressumEN.SetActive(true);  
            impressum2Down.SetActive(false);
            impressumEN2Down.SetActive(true);

            impressumHeadline.text = "CREDITS";
            philosophieHeadline.text = "PHILOSOPHY";
            phiolosphie.SetActive(false);
            phiolosphieEN.SetActive(true);
            groundOsten.text = "E";

            if (aktStep.Contains("N01.") || aktStep.Contains("N02.") || aktStep.Contains("N03.") ||
                aktStep.Contains("N05.") || aktStep.Contains("N07.") || aktStep.Contains("N08.") ||
                aktStep.Contains("N019.")) {
                headline.text = "NOCTURNAL";
            }

            if (aktStep.Contains("Z01.") || aktStep.Contains("S01.") || aktStep.Contains("S02.") ||
                aktStep.Contains("S03.") || aktStep.Contains("S05.")) {
                headline.text = "SUNDIAL";
            }


            if (aktStep.Contains("Z02.") || aktStep.Contains("H")) {
                headline.text = "CELESTIAL GLOBE";
            }
            
            string[] arr = dateText.text.Split(new string[] { "." }, StringSplitOptions.None);
            string monthName = GetMonthName(int.Parse(arr[1]));
            dateText.text = monthName + " " + arr[0]+", "+arr[2];
            
            string[] arrtimeText = timeText.text.Split(new string[] { ":" }, StringSplitOptions.None);
            int tempHours = int.Parse(arrtimeText[0]);
            if (tempHours>=13) {
                tempHours -= 12;
                if (tempHours > 9) {
                    timeText.text = tempHours + ":" + arrtimeText[1] + " pm";
                } else {
                    timeText.text = "0"+tempHours + ":" + arrtimeText[1] + " pm";
                }
            } else if(timeText.text.Contains("Mittag")) {
                timeText.text = "Noon";
            }else{
                if (tempHours > 9) {
                    timeText.text = tempHours + ":" + arrtimeText[1] + " am";
                } else {
                    timeText.text = "0"+tempHours + ":" + arrtimeText[1] + " am";
                }
            }
            
            if (finishState.text.Contains("Nokturnal")&& finishState.text.Length>2) {
                string[] arrFinishState = finishState.text.Split(new string[] { "Du hast " }, StringSplitOptions.None);
                string[] arrFinishState2 = arrFinishState[1].Split(new string[] { "/" }, StringSplitOptions.None);
                finishState.text="You've completed " + arrFinishState2[0] + " of 5 paths of the nocturnal.";
            }else if (finishState.text.Contains("Sonnenuhr")&& finishState.text.Length>2) {
                string[] arrFinishState = finishState.text.Split(new string[] { "Du hast " }, StringSplitOptions.None);
                string[] arrFinishState2 = arrFinishState[1].Split(new string[] { "/" }, StringSplitOptions.None);
                finishState.text="You've completed " + arrFinishState2[0] + " of 5 paths of the sundial.";
            }

            moonDay01.text = "+1 Day";
            moonDay02.text = "+2 Days";
            moonDay03.text = "+3 Days";
            moonDay04.text = "+4 Days";
            moonDay05.text = "+5 Days";
            moonDay06.text = "+6 Days";
            moonDay07.text = "+7 Days";
            
            foreach (Transform child in weekCalendarContentParent.transform) {
                string monthname = child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text;
                if (monthname.Contains("MAI")) {
                    child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "MAY.";
                }
                if (monthname.Contains("OKT")) {
                    child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "OCT.";
                }
                if (monthname.Contains("DEZ")) {
                    child.GetChild(1).transform.GetComponent<TextMeshProUGUI>().text = "DEC.";
                }
            }
            tippHighlight.sprite = tippHighlight_EN;
            jumpBackCechpointMenu.text = "MENU";
        }

        if (aktStep.Contains("N01") || aktStep.Contains("N02")) {
            ChangeSubHeadline(myBubbleList, "N01T", language);
            cPC.InitCheckPointList("N01");
            ChangeFinishInfo(myBubbleList, "N02.22A", language);
        }

        if (aktStep.Contains("N03")) {
            ChangeSubHeadline(myBubbleList, "N03T", language);
            cPC.InitCheckPointList("N03");
            ChangeFinishInfo(myBubbleList, "N03.25A", language);
        }

        if (aktStep.Contains("N05")) {
            ChangeSubHeadline(myBubbleList, "N05T", language);
            cPC.InitCheckPointList("N05");
            ChangeFinishInfo(myBubbleList, "N05.31A", language);
        }

        if (aktStep.Contains("N07")) {
            ChangeSubHeadline(myBubbleList, "N07T", language);
            cPC.InitCheckPointList("N07");
            ChangeFinishInfo(myBubbleList, "N07.27A", language);
        }

        if (aktStep.Contains("N08") || aktStep.Contains("N09")) {
            ChangeSubHeadline(myBubbleList, "N08T", language);
            cPC.InitCheckPointList("N08");
            ChangeFinishInfo(myBubbleList, "N09.24A", language);
        }

        if (aktStep.Contains("Z01")) {
            ChangeSubHeadline(myBubbleList, "Z01T", language);
            cPC.InitCheckPointList("Z01");
            ChangeFinishInfo(myBubbleList, "Z01.21A", language);
        }

        if (aktStep.Contains("S01")) {
            ChangeSubHeadline(myBubbleList, "S01T", language);
            cPC.InitCheckPointList("S01");
            ChangeFinishInfo(myBubbleList, "S01.65A", language);
        }

        if (aktStep.Contains("S02")) {
            ChangeSubHeadline(myBubbleList, "S02T", language);
            cPC.InitCheckPointList("S02");
            ChangeFinishInfo(myBubbleList, "S02.34A", language);
        }

        if (aktStep.Contains("S03")) {
            ChangeSubHeadline(myBubbleList, "S03NEWT", language);
            cPC.InitCheckPointList("S03");
            ChangeFinishInfo(myBubbleList, "S03NEW.35A", language);
        }

        if (aktStep.Contains("S05")) {
            ChangeSubHeadline(myBubbleList, "S05T", language);
            cPC.InitCheckPointList("S05");
            ChangeFinishInfo(myBubbleList, "S05.87A", language);
        }

        if (aktStep.Contains("Z02") || aktStep.Contains("H0")) {
            ChangeSubHeadline(myBubbleList, "Z02T", language);
        }
    }

    private void ChangeSubHeadline(ProcedureController.BubbleList myBubbleList, string targetBubbleID, int language) { 
        int tempStepCount = -1;
        do {
            tempStepCount++;
        } while (myBubbleList.bubbles[tempStepCount].bubbleId != targetBubbleID ||
                 tempStepCount == myBubbleList.bubbles.Length);

        int indexOfAktStep = tempStepCount;

        if (language == 0) {
            subHeadline.text = myBubbleList.bubbles[indexOfAktStep].bubbleOptions[0].optionContent;
            subHeadlineUIText.text = myBubbleList.bubbles[indexOfAktStep].bubbleOptions[0].optionContent;
        } else if (language == 1) {
            subHeadline.text = myBubbleList.bubbles[indexOfAktStep].bubbleOptions[0].optionContent_EN;
            subHeadlineUIText.text = myBubbleList.bubbles[indexOfAktStep].bubbleOptions[0].optionContent_EN;
        }
    }

    private void ChangeFinishInfo(ProcedureController.BubbleList myBubbleList, string targetBubbleID, int language) {
        int tempStepCount = -1;
        do {
            tempStepCount++;
        } while (myBubbleList.bubbles[tempStepCount].bubbleId != targetBubbleID ||
                 tempStepCount == myBubbleList.bubbles.Length);

        int indexOfAktStep = tempStepCount;

        if (language == 0) {
            finishInfo.text = myBubbleList.bubbles[indexOfAktStep].bubbleContent;
        } else if (language == 1) {
            finishInfo.text = myBubbleList.bubbles[indexOfAktStep].bubbleContent_EN;
        }
    }
    public string GetMonthName(int monthNumber) {
        string month = "";
        if (monthNumber == 1) {
            month = "Jan";
        }

        if (monthNumber == 2) {
            month = "Feb";
        }

        if (monthNumber == 3) {
            month = "Mar";
        }

        if (monthNumber == 4) {
            month = "Apr";
        }

        if (monthNumber == 5) {
            month = "May";
        }

        if (monthNumber == 6) {
            month = "Jun";
        }

        if (monthNumber == 7) {
            month = "Jul";
        }

        if (monthNumber == 8) {
            month = "Aug";
        }

        if (monthNumber == 9) {
            month = "Sep";
        }

        if (monthNumber == 10) {
            month = "Oct";
        }

        if (monthNumber == 11) {
            month = "Nov";
        }

        if (monthNumber == 12) {
            month = "Dec";
        }

        return month;
    }

    public string GetMonthNumber(string monthName) {
        string month = "01";
        if (monthName.Contains("Jan")) {
            month = "01";
        }

        if (monthName.Contains("Feb")) {
            month = "02";
        }

        if (monthName.Contains("Mar")) {
            month = "03";
        }

        if (monthName.Contains("Apr")) {
            month = "04";
        }

        if (monthName.Contains("May")) {
            month = "05";
        }

        if (monthName.Contains("Jun")) {
            month = "06";
        }

        if (monthName.Contains("Jul")) {
            month = "07";
        }

        if (monthName.Contains("Aug")) {
            month = "08";
        }

        if (monthName.Contains("Sep")) {
            month = "09";
        }

        if (monthName.Contains("Oct")) {
            month = "10";
        }

        if (monthName.Contains("Nov")) {
            month = "11";
        }

        if (monthName.Contains("Dec")) {
            month = "12";
        }

        return month;
    }
}