﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using RootMotion.FinalIK;
using RootMotion.Demos;

public class model_and_Steam_VR_Controller : MonoBehaviour {
    private GameObject /*pedestrian,*/ cameraRig, actual_Targets, other_Targets;
    public GameObject pedestrian;
    public Transform actualTargets, otherTargets;
    public Transform CameraRig;
    public bool configuration, tracker_configuration;
    private VRIKCalibrationController ik;
    private VRIK ik1;
    public GameObject Head, Pelvis, Left_Hand, Right_Hand, Left_Foot, Right_Foot, Head_ik, Pelvis_ik, Left_Hand_ik, Right_Hand_ik, Left_Foot_ik, Right_Foot_ik, left_hand_target_vrik_calibrator, right_hand_target_vrik_calibrator, left_foot_target_vrik_calibrator, right_foot_target_vrik_calibrator, head_target_vrik_calibrator, pelvis_target_vrik_calibrator;

    // Use this for initialization
    void Start () {
        configuration = false;
        tracker_configuration = false;
    }
	
	// Update is called once per frame
	void Update () {
        if (pedestrian == null)
        {
            pedestrian = GameObject.Find("Sim3");
        }

        if (pedestrian != null && configuration == false)
        {
            cameraRig = Instantiate(CameraRig, new Vector3(pedestrian.transform.position.x, 0f, pedestrian.transform.position.z), Quaternion.identity).gameObject;
            cameraRig.name = "[CameraRig]";
            actual_Targets = Instantiate(actualTargets, new Vector3(pedestrian.transform.position.x, 0f, pedestrian.transform.position.z), Quaternion.identity).gameObject;
            other_Targets = Instantiate(otherTargets, new Vector3(pedestrian.transform.position.x, 0f, pedestrian.transform.position.z), Quaternion.identity).gameObject;
            actual_Targets.name = "Actual Targets";
            other_Targets.name = "Other Targets";
            pedestrian.AddComponent<Tracker_Automatic_Calibrator>();
            pedestrian.AddComponent<TrackerCalibrationController>();
            //pedestrian.AddComponent<Calibration_Script>();

            ik1 = pedestrian.GetComponent<VRIK>();

            ik = this.gameObject.GetComponent<VRIKCalibrationController>();
            Head = GameObject.Find("[CameraRig]/Camera (eye)");
            Pelvis = GameObject.Find("Other Targets/Pelvis_Bone_Tracker");
            Left_Hand = GameObject.Find("Other Targets/Left_Hand_Tracker");
            Right_Hand = GameObject.Find("Other Targets/Right_Hand_Tracker");
            Left_Foot = GameObject.Find("Other Targets/Left_Foot_Tracker");
            Right_Foot = GameObject.Find("Other Targets/Right_Foot_Tracker");

            ik.ik = ik1;
            ik.headTracker = Head.transform;
            ik.bodyTracker = Pelvis.transform;
            ik.leftHandTracker = Left_Hand.transform;
            ik.leftFootTracker = Left_Foot.transform;
            ik.rightHandTracker = Right_Hand.transform;
            ik.rightFootTracker = Right_Foot.transform;

            configuration = true;
        }
        
        if (ik.calibration_done == true)
        {
            cameraRig.transform.eulerAngles = new Vector3(cameraRig.transform.eulerAngles.x,
                                                            90f,
                                                            cameraRig.transform.eulerAngles.z);

            other_Targets.transform.eulerAngles = new Vector3(other_Targets.transform.eulerAngles.x,
                                                            90f,
                                                            other_Targets.transform.eulerAngles.z);

            //var new_scale = ik.data.scale;
            //if (new_scale > 1f)
            //{
            //    var difference = new_scale - 1f;
            //    var new_difference = difference * 3.3f;
            //    var updated_scale = 3.3f + new_difference;
            //    pedestrian.transform.localScale = new Vector3(updated_scale, updated_scale, updated_scale);
            //}

            //else if (new_scale < 1f && new_scale > 0f)
            //{
            //    var difference = 1f - new_scale;
            //    var new_difference = difference * 3.3f;
            //    var updated_scale = 3.3f + new_difference;
            //    pedestrian.transform.localScale = new Vector3(updated_scale, updated_scale, updated_scale);
            //}

            //else
            //    pedestrian.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);

            //cameraRig.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
            //other_Targets.transform.localScale = new Vector3(3.3f, 3.3f, 3.3f);
            ik1.solver.plantFeet = true;

            ik.calibration_done = false;
            tracker_configuration = true;
        }

        if (ik1.solver.leftLeg.bendGoalWeight == 1.0f)
            ik1.solver.leftLeg.bendGoalWeight = 0.0f;
        if (ik1.solver.rightLeg.bendGoalWeight == 1.0f)
            ik1.solver.rightLeg.bendGoalWeight = 0.0f;
    }
}
