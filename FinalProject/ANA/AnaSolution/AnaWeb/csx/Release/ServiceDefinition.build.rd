<?xml version="1.0" encoding="utf-8"?>
<serviceModel xmlns:xsi="http://www.w3.org/2001/XMLSchema-instance" xmlns:xsd="http://www.w3.org/2001/XMLSchema" name="AnaWeb" generation="1" functional="0" release="0" Id="b31ee92f-c467-456f-a623-1c1aa6e5d8cb" dslVersion="1.2.0.0" xmlns="http://schemas.microsoft.com/dsltools/RDSM">
  <groups>
    <group name="AnaWebGroup" generation="1" functional="0" release="0">
      <componentports>
        <inPort name="Ana.Web:Endpoint1" protocol="http">
          <inToChannel>
            <lBChannelMoniker name="/AnaWeb/AnaWebGroup/LB:Ana.Web:Endpoint1" />
          </inToChannel>
        </inPort>
      </componentports>
      <settings>
        <aCS name="Ana.Web:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AnaWeb/AnaWebGroup/MapAna.Web:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="Ana.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AnaWeb/AnaWebGroup/MapAna.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Ana.WebInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AnaWeb/AnaWebGroup/MapAna.WebInstances" />
          </maps>
        </aCS>
        <aCS name="Ana.Worker:DataConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AnaWeb/AnaWebGroup/MapAna.Worker:DataConnectionString" />
          </maps>
        </aCS>
        <aCS name="Ana.Worker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="">
          <maps>
            <mapMoniker name="/AnaWeb/AnaWebGroup/MapAna.Worker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </maps>
        </aCS>
        <aCS name="Ana.WorkerInstances" defaultValue="[1,1,1]">
          <maps>
            <mapMoniker name="/AnaWeb/AnaWebGroup/MapAna.WorkerInstances" />
          </maps>
        </aCS>
      </settings>
      <channels>
        <lBChannel name="LB:Ana.Web:Endpoint1">
          <toPorts>
            <inPortMoniker name="/AnaWeb/AnaWebGroup/Ana.Web/Endpoint1" />
          </toPorts>
        </lBChannel>
      </channels>
      <maps>
        <map name="MapAna.Web:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AnaWeb/AnaWebGroup/Ana.Web/DataConnectionString" />
          </setting>
        </map>
        <map name="MapAna.Web:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AnaWeb/AnaWebGroup/Ana.Web/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAna.WebInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AnaWeb/AnaWebGroup/Ana.WebInstances" />
          </setting>
        </map>
        <map name="MapAna.Worker:DataConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AnaWeb/AnaWebGroup/Ana.Worker/DataConnectionString" />
          </setting>
        </map>
        <map name="MapAna.Worker:Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" kind="Identity">
          <setting>
            <aCSMoniker name="/AnaWeb/AnaWebGroup/Ana.Worker/Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" />
          </setting>
        </map>
        <map name="MapAna.WorkerInstances" kind="Identity">
          <setting>
            <sCSPolicyIDMoniker name="/AnaWeb/AnaWebGroup/Ana.WorkerInstances" />
          </setting>
        </map>
      </maps>
      <components>
        <groupHascomponents>
          <role name="Ana.Web" generation="1" functional="0" release="0" software="C:\Users\gnvilhena\Downloads\AnaSolution11\AnaSolution\AnaWeb\csx\Release\roles\Ana.Web" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaIISHost.exe " memIndex="1792" hostingEnvironment="frontendadmin" hostingEnvironmentVersion="2">
            <componentports>
              <inPort name="Endpoint1" protocol="http" portRanges="80" />
            </componentports>
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Ana.Web&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Ana.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;Ana.Worker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AnaWeb/AnaWebGroup/Ana.WebInstances" />
            <sCSPolicyFaultDomainMoniker name="/AnaWeb/AnaWebGroup/Ana.WebFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
        <groupHascomponents>
          <role name="Ana.Worker" generation="1" functional="0" release="0" software="C:\Users\gnvilhena\Downloads\AnaSolution11\AnaSolution\AnaWeb\csx\Release\roles\Ana.Worker" entryPoint="base\x86\WaHostBootstrapper.exe" parameters="base\x86\WaWorkerHost.exe " memIndex="1792" hostingEnvironment="consoleroleadmin" hostingEnvironmentVersion="2">
            <settings>
              <aCS name="DataConnectionString" defaultValue="" />
              <aCS name="Microsoft.WindowsAzure.Plugins.Diagnostics.ConnectionString" defaultValue="" />
              <aCS name="__ModelData" defaultValue="&lt;m role=&quot;Ana.Worker&quot; xmlns=&quot;urn:azure:m:v1&quot;&gt;&lt;r name=&quot;Ana.Web&quot;&gt;&lt;e name=&quot;Endpoint1&quot; /&gt;&lt;/r&gt;&lt;r name=&quot;Ana.Worker&quot; /&gt;&lt;/m&gt;" />
            </settings>
            <resourcereferences>
              <resourceReference name="DiagnosticStore" defaultAmount="[4096,4096,4096]" defaultSticky="true" kind="Directory" />
              <resourceReference name="EventStore" defaultAmount="[1000,1000,1000]" defaultSticky="false" kind="LogStore" />
            </resourcereferences>
          </role>
          <sCSPolicy>
            <sCSPolicyIDMoniker name="/AnaWeb/AnaWebGroup/Ana.WorkerInstances" />
            <sCSPolicyFaultDomainMoniker name="/AnaWeb/AnaWebGroup/Ana.WorkerFaultDomains" />
          </sCSPolicy>
        </groupHascomponents>
      </components>
      <sCSPolicy>
        <sCSPolicyFaultDomain name="Ana.WebFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyFaultDomain name="Ana.WorkerFaultDomains" defaultPolicy="[2,2,2]" />
        <sCSPolicyID name="Ana.WebInstances" defaultPolicy="[1,1,1]" />
        <sCSPolicyID name="Ana.WorkerInstances" defaultPolicy="[1,1,1]" />
      </sCSPolicy>
    </group>
  </groups>
  <implements>
    <implementation Id="b29b2915-48ae-413e-8d38-64c7e834b3c9" ref="Microsoft.RedDog.Contract\ServiceContract\AnaWebContract@ServiceDefinition.build">
      <interfacereferences>
        <interfaceReference Id="f4685ba3-f2df-4c8e-9642-e78509d98c3b" ref="Microsoft.RedDog.Contract\Interface\Ana.Web:Endpoint1@ServiceDefinition.build">
          <inPort>
            <inPortMoniker name="/AnaWeb/AnaWebGroup/Ana.Web:Endpoint1" />
          </inPort>
        </interfaceReference>
      </interfacereferences>
    </implementation>
  </implements>
</serviceModel>