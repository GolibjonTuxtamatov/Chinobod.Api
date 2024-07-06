using ADotNet.Clients;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks;
using ADotNet.Models.Pipelines.GithubPipelines.DotNets.Tasks.SetupDotNetTaskV1s;

var aDotNetClient = new ADotNetClient();

var githubPipeline = new GithubPipeline
{
    Name = "Chinobod Build Pipline",

    OnEvents = new Events
    {
        Push = new PushEvent
        {
            Branches = new string[] { "master" }
        },

        PullRequest = new PullRequestEvent
        {
            Branches = new string[] { "master" }
        }
    },

    Jobs = new Dictionary<string, Job>
      {
          {
              "build",
              new Job
              {
                  RunsOn = BuildMachines.Windows2019,

                  Steps = new List<GithubTask>
                  {
                      new CheckoutTaskV2
                      {
                          Name = "Checking Out Code"
                      },

                      new SetupDotNetTaskV1
                      {
                          Name = "Setuping .NET",

                          TargetDotNetVersion = new TargetDotNetVersion
                          {
                              DotNetVersion = "8.0.x"
                          }
                      },

                      new RestoreTask
                      {
                          Name = "Restoring Nuget Packages"
                      },

                      new DotNetBuildTask
                      {
                          Name = "Building Project"
                      },

                      new TestTask
                      {
                          Name = "Running Tests"
                      }
                  }
              }
          }
      }
};

string buildScriptPath = "../../../../.github/workflows/dotnet.yml";
string directoryPath = Path.GetDirectoryName(buildScriptPath);

if (!Directory.Exists(directoryPath))
{
    Directory.CreateDirectory(directoryPath);
}

aDotNetClient.SerializeAndWriteToFile(githubPipeline, path: buildScriptPath);