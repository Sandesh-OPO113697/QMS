using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;

public partial class MasterPage : System.Web.UI.MasterPage
{
    protected void Page_Load(object sender, EventArgs e)
    {
        sampling.Visible = false;
        VisibilityNavBar();
    }


    public void VisibilityNavBar()
    {
        List<string> Acess = UserAcesslevel.DistinctFeatureNames;  // Get the user's access level features
      


        if (UserInfo.UserType == "Admin")
        {
            table.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = false;

            table.Visible = true;
            Upload.Visible = true;
            Create.Visible = false;
            UserReport.Visible = false;
        }
        else
        {
            table.Visible = false;
            Upload.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = false;

            Create.Visible = false;
            table.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = false;

            table.Visible = false;
            Upload.Visible = false;
            Create.Visible = false;
            UserReport.Visible = false;
            Create.Visible = false;

            table.Visible = false;
            Upload.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = false;

            Create.Visible = false;
            UserReport.Visible = false;
        }
        if (UserInfo.UserType == "SuperAdmin")
        {
            table.Visible = false;
            Upload.Visible = false;
            UserReport.Visible = false;
            QAForm.Visible = false;

            Create.Visible = true;

            SetVisibilitySuperAdmin("sampling", false);
            SetVisibilitySuperAdmin("ztp_signoff_triggers", false);
            SetVisibilitySuperAdmin("agent_survey", false);
            SetVisibilitySuperAdmin("outlier_management", false);
            SetVisibilitySuperAdmin("work_allocation", false);
            SetVisibilitySuperAdmin("feedback_process", false);
            SetVisibilitySuperAdmin("dispute_process", false);
            SetVisibilitySuperAdmin("calibration", false);
            SetVisibilitySuperAdmin("coaching", false);
            SetVisibilitySuperAdmin("skill_verification", false);
            SetVisibilitySuperAdmin("update_management", false);
            SetVisibilitySuperAdmin("monitoring_workflow", false);
            SetVisibilitySuperAdmin("zt_work_flow", false);

        }

       
        if (UserAcesslevel.DistinctFeatureNames == null)
        {
            // Set visibility for all features to false
            SetVisibility("sampling", false);
            SetVisibility("ztp_signoff_triggers", false);
            SetVisibility("agent_survey", false);
            SetVisibility("outlier_management", false);
            SetVisibility("work_allocation", false);
            SetVisibility("feedback_process", false);
            SetVisibility("dispute_process", false);
            SetVisibility("calibration", false);
            SetVisibility("coaching", false);
            SetVisibility("skill_verification", false);
            SetVisibility("update_management", false);
            SetVisibility("monitoring_workflow", false);
            SetVisibility("zt_work_flow", false);
            return; // Exit early as we don't need to process further
        }
       

        List<string> allFeatures = new List<string>
        {
            "Sampling",
            "Work allocation",
            "Monitoring workflow",
            "Feedback Process",
            "Dispute Process",
            "Calibration",
            "Coaching",
            "Skill verification",
            "Update Management",
            "ZT Work flow",
            "ZTP Sign off triggers",
            "Agent Survey",
            "Outlier Management"
        };
      

        // Iterate over the list of all features and set visibility based on user access
        foreach (var feature in allFeatures)
        {
            // Check if the user has access to the current feature
            if (Acess.Contains(feature))
            {
                // Set the visibility of the corresponding li element to true
                SetVisibility(feature, true);
            }
            else
            {
                // Set the visibility of the corresponding li element to false
                SetVisibility(feature, false);
            }
        } 
    }



    private void SetVisibilitySuperAdmin(string feature, bool isVisible)
    {
        switch (feature)
        {
            case "Sampling":
                sampling.Visible = isVisible;
                break;
            case "work_allocation":
                work_allocation.Visible = isVisible;
                break;
            case "monitoring_workflow":
                monitoring_workflow.Visible = isVisible;
                break;
            case "feedback_process":
                feedback_process.Visible = isVisible;
                break;
            case "dispute_process":
                dispute_process.Visible = isVisible;
                break;
            case "calibration":
                calibration.Visible = isVisible;
                break;
            case "coaching":
                coaching.Visible = isVisible;
                break;
            case "skill_verification":
                skill_verification.Visible = isVisible;
                break;
            case "update_management":
                update_management.Visible = isVisible;
                break;
            case "zt_work_flow":
                zt_work_flow.Visible = isVisible;
                break;
            case "ztp_signoff_triggers":
                ztp_signoff_triggers.Visible = isVisible;
                break;
            case "agent_survey":
                agent_survey.Visible = isVisible;
                break;
            case "outlier_management":
                outlier_management.Visible = isVisible;
                break;
            default:
                break;
        }
    }

    private void SetVisibility(string feature, bool isVisible)
    {
        switch (feature)
        {
            case "sampling":
                sampling.Visible = isVisible;
                break;
            case "Work allocation":
                work_allocation.Visible = isVisible;
                break;
            case "Monitoring workflow":
                monitoring_workflow.Visible = isVisible;
                break;
            case "Feedback Process":
                feedback_process.Visible = isVisible;
                break;
            case "Dispute Process":
                dispute_process.Visible = isVisible;
                break;
            case "Calibration":
                calibration.Visible = isVisible;
                break;
            case "Coaching":
                coaching.Visible = isVisible;
                break;
            case "Skill verification":
                skill_verification.Visible = isVisible;
                break;
            case "Update Management":
                update_management.Visible = isVisible;
                break;
            case "ZT Work flow":
                zt_work_flow.Visible = isVisible;
                break;
            case "ZTP Sign off triggers":
                ztp_signoff_triggers.Visible = isVisible;
                break;
            case "Agent Survey":
                agent_survey.Visible = isVisible;
                break;
            case "Outlier Management":
                outlier_management.Visible = isVisible;
                break;
            default:
                break;
        }
    }


}
