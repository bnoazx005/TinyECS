# Changelog

All notable changes to this project will be documented in this file.

The format is based on [Keep a Changelog](https://keepachangelog.com/en/1.0.0/),
and this project adheres to [Semantic Versioning](https://semver.org/spec/v2.0.0.html).

## [Unreleased]

- Implement different priprities' types for systems. Separate all systems into EARLY, NORMAL and LATE executed.

- Now only a single world's context is supported. In later builds there should be multiple worlds supported.

## [0.4.5] - 2020-08-13

### Fixed

- Issue #24 : **BaseView.mLinkedEntityId** has invalid value when RegisterSubscription is invoked for **BaseStaticView**'s instances

## [0.4.3] - 2020-07-27

### Changed

- Two new types which are **EntityId** and **SystemId** were introduced as strongly typed versions of identifiers.

## [0.4.2] - 2020-07-26

### Added

- A new method IWorldContext.GetUniqueEntity was added.

## [0.4.1] - 2020-07-26 (hotfix)

### Fixed

- Issue #22: **DependencyInjector** always throws **ArgumentNullException** on application's start

## [0.4.0] - 2020-07-25

### Added

- An extension method **CreateAndGetEntity** for **IWorldContext** was added. The main goal for that is to simplify
entity creation.

- A new concept of unique components was introduced. Now if a user implements his/her component from **IUniqueComponent**
then the only instance of this one will exists in the world context.

- Add a new type which is **SystemsPackage** that allows to unite a bunch of related systems together.

### Changed

- Now **DependencyInjector** supports initialization of multiple BaseView components per single GameObject, Issue #20

- Now all **reactive** systems accept all entities that are created despite execution order

### Fixed

- Exceptions handling within EventManager.Notify method was redesigned.

- Fixed an issue #19 "Intercommunicating reactive systems don't receive all messages of the frame"

- Fixed an issue #17 "An entity that was created using previously deteled one has its components"

- Fixed an issue #17

- Increased robustness of the framework


## [0.3.13] - 2020-07-23

### Added

- An extension method **CreateAndGetEntity** for **IWorldContext** was added. The main goal for that is to simplify
entity creation.

## [0.3.12] - 2020-07-21

### Added

- A new concept of unique components was introduced. Now if a user implements his/her component from **IUniqueComponent**
then the only instance of this one will exists in the world context.

## [0.3.11] - 2020-07-19

### Added

- Add a new type which is **SystemsPackage** that allows to unite a bunch of related systems together.

### Fixed 

- Exceptions handling within EventManager.Notify method was redesigned.

## [0.3.10] - 2020-07-11

### Changed

- Now **DependencyInjector** supports initialization of multiple BaseView components per single GameObject, Issue #20

## [0.3.9] - 2020-07-05

### Fixed

- Fixed an issue #19 "Intercommunicating reactive systems don't receive all messages of the frame"

### Changed

- Now all **reactive** systems accept all entities that are created despite execution order

## [0.3.8] - 2020-06-21

### Fixed

- Fixed an issue #17 "An entity that was created using previously deteled one has its components"

## [0.3.5] - 2020-06-20

### Fixed

- Fixed an issue #17

- Increased robustness of the framework

## [0.3.0] - 2019-06-12

### Added

- A components iterator that provides easy way of enumerating over all components that some entity has

- Add custom debug inspectors for WorldContextsManager, EntityObserver and SystemManagerObserver types

- Implement ToString() method for Entity type

- Add template project for Unity3D and corresponding tutorial sample, which demonstrates how to integrate TinyECS into Unity3D

### Fixed

- Fixed an issue "Created with GameObjectFactory entities don't have TViewComponent attached to them" #11

- Fixed an issue "Reactive systems don't response on to events that are generated with IUpdateSystem implementations" #6

### Changed

- Now all reactive systems are executed after all IUpdateSystems

## [0.2.21] - 2019-05-30

### Added

- A components iterator that provides easy way of enumerating over all components that some entity has

- Add custom debug inspectors for WorldContextsManager, EntityObserver and SystemManagerObserver types

- Implement ToString() method for Entity type

### Fixed

- Now you can subscribe same reference to a system in different roles. For example, as initializing system and as update system at the same time

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