import React, { useState, useEffect } from 'react';
import {
    AppBar,
    Box,
    Drawer,
    Fab,
    IconButton,
    List,
    ListItem,
    ListItemButton,
    ListItemIcon,
    ListItemText,
    Toolbar,
    Typography,
    useMediaQuery,
    useTheme,
    Paper
} from '@mui/material';
import { DragDropContext, Droppable, Draggable } from '@hello-pangea/dnd';
import { projectApi, milestoneApi } from '../services';



import { useNavigate } from 'react-router-dom';

import MilestoneList from './MilestoneList';
/* 
mock data
    mock milestones
    mock projects

    join projects to milestones for mock data

    */

const mockMilestones = [
    { id: 101, name: "development" },
    { id: 102, name: "preproduction" },
    { id: 103, name: "production" },
    { id: 104, name: "postproduction" },
    { id: 105, name: "invoiced" },
    { id: 106, name: "completed" }
]

const mockData = [
    {
        // project
        project_id: 0,
        project_short_name: "CedarPeak Product Promo",
        project_title: "Titanium Cookset Product Launch Video",

        // milestone
        milestone_id: 101
    },
    {
        // project
        project_id: 1,
        project_short_name: "E Coffee Campaign",
        project_title: "Emberlane Coffee Autumn Blend Social Media Campaign",

        // milestone
        milestone_id: 102
    },
    {
        // project
        project_id: 2,
        project_short_name: "BlossomWell promo",
        project_title: "BlossomWell Wellness App Promotional Video",

        // milestone
        milestone_id: 103
    },
    {
        // project
        project_id: 3,
        project_short_name: "PulseTrack Launch",
        project_title: "PulseTrack Inventory Management System Industrial Video",

        // milestone
        milestone_id: 101
    },
    {
        // project
        project_id: 4,
        project_short_name: "NovaBloom Candles Ad",
        project_title: "NovaBloom Scented Candles TV Ad",

        // milestone
        milestone_id: 102
    },
    {
        // project
        project_id: 5,
        project_short_name: "IronWeave Safety Video",
        project_title: "IronWeave Safety Equipment Training Video",

        // milestone
        milestone_id: 103
    }


]



const Board = () => {
    const navigate = useNavigate();

    // get milestonesList


    // // use state 
    const [milestones, setMilestones] = useState([]);
    const [projects, setProjects] = useState([]);

    useEffect(() => {
        loadMilestones();
        loadProjects();
    }, []);

    const loadMilestones = async () => {
        try {
            const data = await milestoneApi.getAll();
            data.sort((a, b) => a.position - b.position);
            console.log(data)
            setMilestones(data);
            // setMilestones(data);
            return data;
        } catch (error) {
            console.error('Failed to load milestones:', error);
            setMilestones(mockMilestones)
            return [];
        }
    }

    const loadProjects = async () => {
        try {
            const data = await projectApi.getAll();
            setProjects(data);
            return data;
        } catch (error) {
            console.error('Failed to load projects:', error);
            setProjects(mockData);
            return [];
        }
    }

    const handlePositionChange = async (id, newPosition, dragType, destinationMilestoneId) => {
        newPosition++;
        if (dragType === 'milestone') {
            try {
                await milestoneApi.changePosition(id, newPosition);
                // Optionally reload milestones to reflect changes
                await loadMilestones();
                await loadProjects();
            } catch (error) {
                console.error('Failed to update milestone position:', error);
            }
        }
        else {
            try {
                // For projects, use the milestone ID from the destination, not the index
                const projectId = parseInt(id.replace('p', ''));
                const milestoneId = parseInt(destinationMilestoneId);
                await projectApi.updateMilestone(projectId, milestoneId);
                await loadMilestones();
                await loadProjects();

            } catch (error) {
                console.error('Failed to update project milestone:', error);
            }
        }


    }

    // fetch milestones from api

    // helper functions

    return (
        // Drag Drop Context

        // Droppable Component


        // map milestonesList components with draggable element


        <Box sx={{ p: 3, overflowX: 'auto' }}>
            <DragDropContext onDragStart={() => console.log('drag started')} onDragEnd={async (results) => {
                console.log('drag ended', results);

                // Check if dropped in a valid location
                if (!results.destination) {
                    console.log('Dropped outside valid area');
                    return;
                }

                // Check if position actually changed
                if (results.source.droppableId === results.destination.droppableId &&
                    results.source.index === results.destination.index) {
                    console.log('No position change');
                    return;
                }

                console.log("handle position change for milestone id:", results.draggableId, "to new index:", results.destination.index);
                handlePositionChange(results.draggableId, results.destination.index, results.type, results.destination.droppableId);
            }}>
                <Droppable droppableId="boardDroppable" type="milestone" direction="horizontal">
                    {(provided) => (
                        <Box className="board"
                            sx={{ display: 'flex', gap: 3 }}
                            ref={provided.innerRef}
                            {...provided.droppableProps}
                        >
                            {Object.values(milestones).map((milestone, index) =>
                            (
                                <Draggable draggableId={milestone.id.toString()} key={milestone.id} index={index}>
                                    {(provided) => (
                                        <Box
                                            ref={provided.innerRef}
                                            {...provided.draggableProps}
                                            {...provided.dragHandleProps}
                                        >
                                            <MilestoneList key={milestone.id} milestone={milestone} projects={projects.filter(project => project.milestoneId === milestone.id)} navigate={navigate} index={index} />


                                        </Box>
                                    )}
                                </Draggable>
                                // <MilestoneList key={milestone.id} milestone={milestone} projects={mockData.filter(project => project.milestone_id === milestone.id)} navigate={navigate} index={index} />
                            ))
                            }
                            {provided.placeholder}
                        </Box>
                    )}

                </Droppable>

            </DragDropContext>
            {/* <Box sx={{ display: 'flex', gap: 3 }}>
                {milestones.map((milestone) => (
                    <MilestoneList key={milestone.id} milestone={milestone} projects={mockData.filter(project => project.milestone_id === milestone.id)} navigate={navigate} />
                ))
                }
            </Box> */}
        </Box >
    )

}

export default Board;