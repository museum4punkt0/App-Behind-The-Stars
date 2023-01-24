#region Description
/*In N03 wird ein Wochenslider benötigt mit dem eine Jahresdrehung durchgeführt werden soll.
 * Der Wochen-Slider soll mit dem aktuellen Tag beginnen und in einem Jahr enden (plus 3-4 Wochen, um den Slider nicht
 * hart enden zu lassen) die letzten 2 Wochen sollen tageweise durchlaufen werden können.
 * Das Skript initialisiert also zum Anwendungsbeginn den WochenSlider für N03.
 */
#endregion

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using TMPro;

public class WeekCreator : MonoBehaviour {
#region public Variables

    public GameObject weekList;
    
    public List<DateTime> weekDays = new List<DateTime>();

#endregion

    private bool targetWeekReached = false;

    void Start() {
        DateTime startDate = new DateTime(DateTime.Now.Year, DateTime.Now.Month, DateTime.Now.Day);
        var dates = new List<DateTime>();
        var @from = DateTime.Today;
        @from = DateTime.Today.AddDays(-14);

        var to = startDate.AddDays(450);
        for (var dt = @from; dt <= to; dt = dt.AddDays(1)) {
            dates.Add(dt);
        }

        System.DateTime targetDate = startDate.AddDays(357);

        int checkDay = 0;
        if (DateTime.Today.DayOfWeek == DayOfWeek.Monday) {
            checkDay = 1;
        } else if (DateTime.Today.DayOfWeek == DayOfWeek.Tuesday) {
            checkDay = 2;
        } else if (DateTime.Today.DayOfWeek == DayOfWeek.Wednesday) {
            checkDay = 3;
        } else if (DateTime.Today.DayOfWeek == DayOfWeek.Thursday) {
            checkDay = 4;
        } else if (DateTime.Today.DayOfWeek == DayOfWeek.Friday) {
            checkDay = 5;
        } else if (DateTime.Today.DayOfWeek == DayOfWeek.Saturday) {
            checkDay = 6;
        } else if (DateTime.Today.DayOfWeek == DayOfWeek.Sunday) {
            checkDay = 0;
        }

        var alldays = dates.Where(x => (int) x.DayOfWeek == checkDay);
        int i = 0;
        foreach (var day in alldays) {
            try {
                if (day >= targetDate && !targetWeekReached) {
                    for (int j = 0; j < 14; j++) {
                        System.DateTime tempday = day.AddDays(j);
                        weekList.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>()
                            .text = tempday.Day.ToString();
                        string monthName = "";
                        if (tempday.Month == 1) {
                            monthName = "JAN.";
                        }

                        if (tempday.Month == 2) {
                            monthName = "FEB.";
                        }

                        if (tempday.Month == 3) {
                            monthName = "MAR.";
                        }

                        if (tempday.Month == 4) {
                            monthName = "APR.";
                        }

                        if (tempday.Month == 5) {
                            monthName = "MAI.";
                        }

                        if (tempday.Month == 6) {
                            monthName = "JUN.";
                        }

                        if (tempday.Month == 7) {
                            monthName = "JUL.";
                        }

                        if (tempday.Month == 8) {
                            monthName = "AUG.";
                        }

                        if (tempday.Month == 9) {
                            monthName = "SEP.";
                        }

                        if (tempday.Month == 10) {
                            monthName = "OKT.";
                        }

                        if (tempday.Month == 11) {
                            monthName = "NOV.";
                        }

                        if (tempday.Month == 12) {
                            monthName = "DEZ.";
                        }

                        weekList.transform.GetChild(i).transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>()
                            .text = monthName;
                        weekDays.Add(tempday);
                        i++;
                    }

                    targetWeekReached = true;
                } else {
                    weekList.transform.GetChild(i).transform.GetChild(0).transform.GetComponent<TextMeshProUGUI>()
                        .text = day.Day.ToString();
                    string monthName = "";

                    if (day.Month == 1) {
                        monthName = "JAN.";
                    }

                    if (day.Month == 2) {
                        monthName = "FEB.";
                    }

                    if (day.Month == 3) {
                        monthName = "MAR.";
                    }

                    if (day.Month == 4) {
                        monthName = "APR.";
                    }

                    if (day.Month == 5) {
                        monthName = "MAI";
                    }

                    if (day.Month == 6) {
                        monthName = "JUN.";
                    }

                    if (day.Month == 7) {
                        monthName = "JUL.";
                    }

                    if (day.Month == 8) {
                        monthName = "AUG.";
                    }

                    if (day.Month == 9) {
                        monthName = "SEP.";
                    }

                    if (day.Month == 10) {
                        monthName = "OKT.";
                    }

                    if (day.Month == 11) {
                        monthName = "NOV.";
                    }

                    if (day.Month == 12) {
                        monthName = "DEZ.";
                    }

                    weekList.transform.GetChild(i).transform.GetChild(1).transform.GetComponent<TextMeshProUGUI>()
                        .text = monthName;
                    weekDays.Add(day);
                    i++;
                }
            } catch (ArgumentOutOfRangeException ex) {
            }
        }
    }

    public List<DateTime> GetWeekDayList() {
        return weekDays;
    }
}