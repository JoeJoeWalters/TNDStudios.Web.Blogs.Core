node {
	stage('Checkout')
	{
		checkout scm
	}

	stage('Build')
	{
		bat 'nuget restore TNDStudios.sln'
        def msbuild = tool name: 'MSBuild', type: 'hudson.plugins.msbuild.MsBuildInstallation'
		bat "\"${msbuild}\" TNDStudios.sln /p:Configuration=Release /p:Platform=\"Any CPU\" /p:ProductVersion=1.0.0.${env.BUILD_NUMBER}"
	}
	stage('Archive')
	{
		archive 'TNDStudios.Web.Blogs/bin/Release/**'
	}
}