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

        }

       
        if (UserAcesslevel.DistinctFeatureNames == null)
        {
            // Set visibility for all features to false
            SetVisibility("Sampling", false);
            SetVisibility("Work allocation", false);
            SetVisibility("Monitoring workflow", false);
            SetVisibility("Feedback Process", false);
            SetVisibility("Dispute Process", false);
            SetVisibility("Calibration", false);
            SetVisibility("Coaching", false);
            SetVisibility("Skill verification", false);
            SetVisibility("Update Management", false);
            SetVisibility("ZT Work flow", false);
            SetVisibility("ZTP Sign off triggers", false);
            SetVisibility("Agent Survey", false);
            SetVisibility("Outlier Management", false);
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

    private void SetVisibility(string feature, bool isVisible)
    {
        switch (feature)
        {
            case "Sampling":
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
