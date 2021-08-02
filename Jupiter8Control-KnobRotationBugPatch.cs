using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Jupiter8Control : MonoBehaviour
{
    private Ray ray;
    private RaycastHit hit;
    private float delta;
    private float sensitivity;
    private Vector3 origin;
    private bool selected;
    private bool first;
    private UnityEngine.UI.Text HUD;
    private GameObject control;
    private Dictionary<string, Vector3> origins;

    void Start()
    {
        hit = new RaycastHit();
        ray = new Ray();
        delta = 0.0f;
        sensitivity = 0.1f;
        origin = new Vector3(0.0f, 0.0f, 0.0f);
        selected = false;
        first = true;
        HUD = GameObject.Find("/HUD/Text").GetComponent<UnityEngine.UI.Text>();
        control = null;
        origins = new Dictionary<string, Vector3>();
    }

    void Update()
    {
        selected = Input.GetMouseButton(0);
        ray = Camera.main.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0.0f));
        if (Physics.Raycast(ray, out hit) && (!Input.GetMouseButton(0)))
        {
            control = hit.collider.transform.gameObject;
            HUD.text = control.name;
        }
        else if (!selected) { HUD.text = "Space"; }
        if (control != null) //Support a variety of control classes here
        {
            if (selected)
            {
                if (control.name.StartsWith("Button"))
                {
                    if (first)
                    {
                        if (!(origins.TryGetValue(control.name, out origin)))
                        {
                            origin = control.transform.position;
                            origins.Add(control.name, origin);
                        }
                    }
                    control.transform.position = new Vector3(origin.x, origin.y - 0.01f, origin.z);
                }
                if (control.name.StartsWith("Key"))
                {
                    if (first)
                    {
                        if (!(origins.TryGetValue(control.name, out origin)))
                        {
                            origin = control.transform.eulerAngles;
                            origins.Add(control.name, origin);
                        }
                    }
                    control.transform.eulerAngles = new Vector3(origin.x, origin.y, origin.z + 2.0f);
                }
                if (control.name.StartsWith("Fader"))
                {
                    if (first)
                    {
                        if (!(origins.TryGetValue(control.name, out origin)))
                        {
                            origin = control.transform.position;
                            origins.Add(control.name, origin);
                        }
                        else { delta = control.transform.position.z - origin.z; }
                    }
                    delta += (Input.GetAxis("Mouse Y") * sensitivity);
                    control.transform.position = new Vector3(origin.x, origin.y, Mathf.Clamp(origin.z + delta, origin.z - 0.1f, origin.z));
                }
                if (control.name.StartsWith("Knob"))
                {
                    if (first)
                    {
                        if (!(origins.TryGetValue(control.name, out origin)))
                        {
                            origin = control.transform.eulerAngles;
                            origins.Add(control.name, origin);
                        }
                        else { delta = control.transform.eulerAngles.y; if (delta > 180.0f) { delta -= 360.0f; } } //- origin.y
                    }
                    delta += (Input.GetAxis("Mouse Y") * sensitivity * 120.0f);
                    if (first) { Debug.Log(delta); }
                    control.transform.eulerAngles = new Vector3(origin.x, Mathf.Clamp(origin.y + delta, origin.y - 160.0f, origin.y + 160.0f), origin.z);
                }
                if (control.name.StartsWith("Toggle"))
                {
                    if (first)
                    {
                        if (!(origins.TryGetValue(control.name, out origin)))
                        {
                            origin = control.transform.eulerAngles;
                            origins.Add(control.name, origin);
                        }
                        else { delta = origin.x - control.transform.eulerAngles.x; }
                    }
                    delta += (Input.GetAxis("Mouse Y") * sensitivity * 60.0f);
                    if ((control.name == "Toggle13") || (control.name == "Toggle14") || (control.name == "Toggle15"))
                    {
                        control.transform.eulerAngles = new Vector3(Mathf.Clamp(origin.x - delta, origin.x - 60.0f, origin.x), origin.y, origin.z);
                    }
                    else
                    {
                        control.transform.eulerAngles = new Vector3(Mathf.Clamp(origin.x - delta, origin.x, origin.x + 60.0f), origin.y, origin.z);
                    }
                }
                if (control.name.StartsWith("Wheel"))
                {
                    if (first)
                    {
                        if (!(origins.TryGetValue(control.name, out origin)))
                        {
                            origin = control.transform.position;
                            origins.Add(control.name, origin);
                        }
                        else { delta = control.transform.position.x - origin.x; }
                    }
                    delta += (Input.GetAxis("Mouse X") * sensitivity);
                    control.transform.position = new Vector3(Mathf.Clamp(origin.x + delta, origin.x - 0.01f, origin.x + 0.01f), origin.y, origin.z);
                }
                first = false;
            }
            if (!selected)
            {
                if (control.name.StartsWith("Button"))
                {
                    if (origins.TryGetValue(control.name, out origin)) { control.transform.position = origin; }
                }
                if (control.name.StartsWith("Key"))
                {
                    if (origins.TryGetValue(control.name, out origin)) { control.transform.eulerAngles = origin; }
                }
                delta = 0.0f;
                first = true;
            }
        }
    }
}
