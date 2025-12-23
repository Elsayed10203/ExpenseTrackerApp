# .NET MAUI CI/CD Documentation

This repository contains CI/CD pipelines for the ExpenseTrackerApp .NET MAUI application.

## Available Pipelines

### 1. GitHub Actions - Basic CI (`maui-ci.yml`)
Runs on every push and pull request to master/main/develop branches.

**Features:**
- Builds Android and iOS versions
- Runs on separate runners (Windows for Android, macOS for iOS)
- Uploads build artifacts
- Basic testing support (commented out)

### 2. GitHub Actions - Advanced CD (`maui-cd.yml`)
Runs on version tags (e.g., `v1.0.0`) for releases.

**Features:**
- Signed Android APK generation
- iOS IPA generation with certificates
- Automatic GitHub Release creation
- Artifact retention for 90 days

### 3. Azure DevOps Pipeline (`azure-pipelines.yml`)
Multi-stage pipeline for comprehensive CI/CD.

**Features:**
- Build stage for both platforms
- Release stage triggered on version tags
- Artifact publishing
- Multi-platform support

## Setup Instructions

### GitHub Actions Setup

#### Basic CI (maui-ci.yml)
No additional setup required. Push code to trigger the build.

#### Advanced CD (maui-cd.yml)

**Android Signing:**
1. Generate or use existing Android keystore:
   ```bash
   keytool -genkey -v -keystore android_keystore.keystore -alias key -keyalg RSA -keysize 2048 -validity 10000
   ```

2. Convert keystore to Base64:
   ```bash
   base64 android_keystore.keystore > android_keystore_base64.txt
   ```

3. Add GitHub Secrets:
   - `ANDROID_KEYSTORE_BASE64`: Content of android_keystore_base64.txt
   - `ANDROID_KEYSTORE_PASSWORD`: Your keystore password

**iOS Signing:**
1. Export your Apple Developer certificate as `.p12` file

2. Convert certificate to Base64:
   ```bash
   base64 certificate.p12 > certificate_base64.txt
   ```

3. Export provisioning profile and convert to Base64:
   ```bash
   base64 profile.mobileprovision > profile_base64.txt
   ```

4. Add GitHub Secrets:
   - `APPLE_CERTIFICATE_BASE64`: Content of certificate_base64.txt
   - `APPLE_CERTIFICATE_PASSWORD`: Your certificate password
   - `APPLE_PROVISIONING_PROFILE_BASE64`: Content of profile_base64.txt

### Azure DevOps Setup

1. Create a new pipeline in Azure DevOps
2. Select "Existing Azure Pipelines YAML file"
3. Choose `azure-pipelines.yml`
4. For signed builds, add the following variables as secrets:
   - Android signing variables
   - iOS signing variables

## Triggering Builds

### Automatic Triggers

**Basic CI:**
- Push to master, main, or develop branches
- Pull requests to master, main, or develop branches

**Release CD:**
- Create and push a version tag:
  ```bash
  git tag v1.0.0
  git push origin v1.0.0
  ```

### Manual Triggers
Both GitHub Actions workflows support manual triggering via `workflow_dispatch`.

## Build Outputs

### Android
- **Debug APK**: `ExpenseTrackerApp/bin/Release/net10.0-android/*.apk`
- **Signed APK**: `ExpenseTrackerApp/bin/Release/net10.0-android/publish/*.apk`

### iOS
- **App Bundle**: `ExpenseTrackerApp/bin/Release/net10.0-ios/**/*.app`
- **IPA**: `ExpenseTrackerApp/bin/Release/net10.0-ios/ios-arm64/publish/*.ipa`

## Customization

### Change .NET Version
Update the `DOTNET_VERSION` or `dotnetVersion` variable in the YAML files.

### Add Tests
Uncomment the test section in `maui-ci.yml` when test projects are added:
```yaml
- name: Run Tests
  run: dotnet test --no-restore --verbosity normal
```

### Change Target Frameworks
Update build commands if you add/remove target frameworks (e.g., Windows):
```yaml
run: dotnet build ${{ env.PROJECT_PATH }} -c Release -f net10.0-windows --no-restore
```

## Troubleshooting

### Common Issues

**Issue**: Workload installation fails
- **Solution**: The `--ignore-failed-sources` flag helps, but ensure proper .NET SDK version

**Issue**: Android build fails on macOS
- **Solution**: Use Windows runners for Android builds (as configured)

**Issue**: iOS build fails on Windows
- **Solution**: Use macOS runners for iOS builds (as configured)

**Issue**: Signing fails
- **Solution**: Verify all secrets are correctly set and Base64 encoded properly

### Logs and Artifacts
- GitHub Actions: Check "Actions" tab in your repository
- Azure DevOps: Check "Pipelines" section

## Best Practices

1. **Version Tagging**: Use semantic versioning (v1.0.0, v1.1.0, etc.)
2. **Branch Protection**: Enable branch protection for master/main
3. **Secret Rotation**: Regularly update signing certificates and passwords
4. **Artifact Cleanup**: Configure retention policies to save storage
5. **Test Coverage**: Add unit and integration tests before production

## Resources

- [.NET MAUI Documentation](https://docs.microsoft.com/dotnet/maui/)
- [GitHub Actions Documentation](https://docs.github.com/actions)
- [Azure DevOps Pipelines](https://docs.microsoft.com/azure/devops/pipelines/)
