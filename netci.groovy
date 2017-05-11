// Import the utility functionality.
import jobs.generation.Utilities;
import jobs.generation.JobReport;

def project = GithubProject
def branch = GithubBranchName

// Generate the builds for debug and release, commit and PRJob
[true, false].each { isPR -> // Defines a closure over true and false, value assigned to isPR
    ['Debug', 'Release'].each { configuration ->
        ['Windows_NT', 'Ubuntu16.04', 'OSX10.12'].each { osName ->   
            def isPRString = ""
            if (isPR)
            {
                isPRString = "_prtest"
            }
            // Determine the name for the new job.  The first parameter is the project,
            // the second parameter is the base name for the job, and the last parameter
            // is a boolean indicating whether the job will be a PR job.  If true, the
            // suffix _prtest will be appended.
            def newJobName = "${osName.toLowerCase()}_${configuration.toLowerCase()}${isPRString}"

            // Create a new job with the specified name.  The brace opens a new closure
            // and calls made within that closure apply to the newly created job.
            def newJob = job(newJobName) {
                
                // This opens the set of build steps that will be run.
                steps {
                    if (osName == 'Windows_NT') {
                        // Indicates that a batch script should be run with the build string
                        batchFile("call build.cmd ${configuration}")
                    }
                    else if (osName == 'OSX10.12') {
                        shell("HOME=\$WORKSPACE/tempHome ./build.sh ${configuration}")
                    }
                    else {
                        // shell (for unix scripting)
                        shell("./build.sh ${configuration}")
                    }
                }
            }
            
            Utilities.setMachineAffinity(newJob, osName, 'latest-or-auto')
            Utilities.standardJobSetup(newJob, project, isPR, "*/${branch}")
            // Archive only on commit builds.
            if (isPR) {
                // Set PR trigger, we run Windows_NT, Ubuntu 16.04, and OSX on every PR.
                Utilities.addGithubPRTriggerForBranch(newJob, branch, "Innerloop ${osName} ${configuration} Build and Test")
            }
            else {
                Utilities.addGithubPushTrigger(newJob)
            }
        }
    }
}

// Generate the job report
JobReport.Report.generateJobReport(out)