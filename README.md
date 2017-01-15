# **CRM Solution Manager**

A tool for Dynamics CRM 2016 unmanaged solutions and patches to manage.


### Problem

CRM 2016 does not support unmanaged patches to be imported into other than the source organization. Aim of this project is to help Dynamics CRM development cycle with unmanaged solutions/patches and make it more maintainable for releases. Process is built on top of out of the box **Patching** mechanism (introduced in CRM 2016).

### Solution

CRMSolutionManager brings 2 new features, **Patches To Solution** and **Apply Solution Upgrade**;
* Patches To Solution, works on selected 1 solution in **Solutions** screen (**e.g. Solution A**). Below are the steps;
 * Checks patches belongs to selected solution (**Solution A**),
 * Creates a new solution with the main solution name and timestamp concatnated (**Solution B yyyy_MM_dd_hh_mm_ss**),
 * Moves all the components into newly created solution
 * Executes CloneAsSolutionRequest on the main solution (Solution A), which leads all the patches to be merged with the main solution,
 * In conclusion, Solution B is unmanaged and contains only the components which are worked on recently.
 * Decreases solution import time

![Patch To Solution](https://github.com/osmanium/CRMSolutionManager/blob/develop/Docs/patch_to_solution.gif)

* Apply Solution Upgrade, works on selected 1 solution in **Solutions** screen (**e.g. Solution A**). Below are the steps;
 * Let's assume Solution B, which contains recent development, is deployed on clients servers,
 * This means there can be multiple deployments and in time this list can grow,
 * To prevent list grow in each deployment, solutions can be merged with the main solution,
 * This feature moves all the components to main solution,
 * Upgrades main solution's version,
 * Revemos the upgrade solution

![Apply Solution Upgrade](https://github.com/osmanium/CRMSolutionManager/blob/develop/Docs/apply_solution_upgrade.gif)

### Installation

Lates release can be found in [Releases](https://github.com/osmanium/CRMSolutionManager/releases) page.
Release contains only a managed solution to be imported into CRM
After importing, navigate to Settings -> Soltuions page and it can be found in the ribbon.


### Releases

* v0.1.0.0 - Beta












