using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Experimental.XR;
using UnityEngine.XR.ARFoundation;
using UnityEngine.XR.ARSubsystems;
using UnityEngine.UI;

public class TapToPlaceObject : MonoBehaviour
{

    [Space]
    public GameObject m_objecttoplace;
    private ARRaycastManager m_arraycastmanager;
    private bool m_vaildposition;

    private Pose m_pose;
    [Space]
    public GameObject m_placementindicator;
    private Vector3 m_centerpos;
    private List<ARRaycastHit> m_hits;

    private Camera m_maincamera;

    [Space]
    public Text m_status;
    private GameObject m_obj;

    private void Start()
    {
        m_arraycastmanager = FindObjectOfType<ARRaycastManager>();
        m_maincamera = Camera.main;
    }


    private void Update()
    {
        _UpdatePlacementPose();
        _UpdatePlacementIndicator();

        if (m_vaildposition)
        {
            if (Input.touchCount>0)
            {
                m_status.text="Object Placed ";
                m_obj = Instantiate(m_objecttoplace, m_pose.position, m_pose.rotation);
                m_obj.transform.localScale = Vector3.one;
            }
        }

    }

    private void _UpdatePlacementIndicator()
    {
        if (m_vaildposition)
        {
            m_status.text = "ValidPositionFouund";
            m_placementindicator.SetActive(true);
            m_placementindicator.transform.SetPositionAndRotation(m_pose.position, m_pose.rotation);
        }
        else
        {
            m_status.text = "Placement Not Available";
            Debug.Log("Placement Not Available");
            m_placementindicator.SetActive(false);
        }
    }

    private void _UpdatePlacementPose()
    {
        m_centerpos = m_maincamera.ViewportToScreenPoint(new Vector3(0.5f, 0.5f));

        m_hits = new List<ARRaycastHit>();
        m_arraycastmanager.Raycast(m_centerpos, m_hits, TrackableType.Planes);

        m_vaildposition = m_hits.Count > 0;

        if (m_vaildposition)
        {
            m_pose = m_hits[0].pose;
        }
        else
        {
            m_status.text = "No Hits";
        }
    }
}
