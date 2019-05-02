# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

- Implement different priprities' types for systems. Separate all systems into EARLY, NORMAL and LATE executed.

- Now only a single world's context is supported. In later builds there should be multiple worlds supported.

## [0.2.0] - 2019-05-02

### Added

- A bunch of helper types for TinyECS were added into the project to implement an integration with Unity3D.

- Support of static and dynamically created views was implemented.

- A new type of entities was introduced which are disposable entities.

### Changed

- Now event manager separates types of delivering events to their listeners. Single- and broadcasting are now supported.

### Fixed

- IEntityManager.GetEntitiesWithAll's implementation was fixed. 

- EntityManager.DestroyEntity's implementation was fixed. Now it correctly destroys given entities.